using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;
using Utils;
using VR.Enums;
using VR.Interactions.Interactors;

namespace VR.Interactions.Network
{
	// Class based on photon fusion implementation of network physical grabbable
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_GRABBABLE)]
	public class NetworkPhysicsGrabbable : NetworkGrabbable, IInputAuthorityLost
	{
		#region Properties

		protected override NetworkGrabber CurrentGrabber
		{
			get
			{
				if (m_willReceiveInputAuthority)
				{
					return m_incomingGrabber;
				}

				if (DetailedGrabInfo.GrabbingUser != PlayerRef.None)
				{
					return GrabberForId(DetailedGrabInfo.GrabInteractorId);
				}

				return null;
			}
		}

		[Networked] private NetworkDetailedGrabInfo DetailedGrabInfo { get; set; }
		[Networked] private NetworkBool IsColliding { get; set; } = false;

		// PID Memorization
		[Networked] private Vector3 PidLastError { get; set; }
		[Networked] private Vector3 PidErrorIntegration { get; set; }

		#endregion

		#region SerializeFields

		[SerializeField] private NetworkRigidbody3D m_networkRigidbody;
		[SerializeField] private CustomPhysicsGrabbable m_grabbable;
		[SerializeField] private bool m_displayInRemoteTimeFrameWhenGrabbed = true;

		#endregion

		#region PrivateFields

		private ChangeDetector m_changeDetector;
		// private bool m_isPseudoHapticDisplayed = false;
		private List<Localization> m_lastLocalizations = new();
		private bool m_willReceiveInputAuthority;
		private ERigPart m_previousGrabbingSide = ERigPart.None;

		// Stored to cancel the wait for input authority if someone catched the object quickly just after us
		private float m_inputAuthorityChangeRequestTime;

		// Stored to avoid anticipating a grab too early in resim
		private Tick m_inputAuthorityChangeRequestTick;
		private NetworkGrabInfo m_incomingGrabInfo;
		private NetworkGrabber m_incomingGrabber;
		private Dictionary<(PlayerRef player, ERigPart side), NetworkGrabber> m_cachedGrabbers = new();

		#endregion

		#region InterfaceImplementations

		#region IInputAuthorityLost

		public void InputAuthorityLost()
		{
			// When using Object.AssignInputAuthority, SetIsSimulated will be reset to false. as we want the object to remain simulated (see Spawned), we have to set it back
			Runner.SetIsSimulated(Object, true);
		}

		#endregion

		#endregion

		#region NestedTypes

		private struct Localization
		{
			#region PublicFields

			public float Timestamp;
			public Vector3 Position;
			public Quaternion Rotation;

			#endregion
		}

		#endregion

		#region Feedback configuration

		[Serializable]
		public struct PseudoHapticFeedbackConfiguration
		{
			#region PublicFields

			public bool EnablePseudoHapticFeedback;
			public float MinNonContactingDissonance;
			public float MinContactingDissonance;
			public float MaxDissonanceDistance;
			public float VibrationDuration;

			#endregion
		}

		[Header("Feedback configuration")] [SerializeField] private PseudoHapticFeedbackConfiguration m_pseudoHapticFeedbackConfiguration = new()
		{
			EnablePseudoHapticFeedback = true,
			MinNonContactingDissonance = 0.05f,
			MinContactingDissonance = 0.005f,
			MaxDissonanceDistance = 0.60f,
			VibrationDuration = 0.06f
		};

		#endregion

		#region NetworkGrabbable

		// Will be called by the host and by the grabbing user (the input authority of the NetworkGrabber) upon NetworkGrabber.GrabInfo change detection
		//  For other users, will be called by the local NetworkGrabbable.DetailedGrabInfo change detection
		public override void Grab(NetworkGrabber newGrabber, NetworkGrabInfo newGrabInfo)
		{
			if (Object.InputAuthority != newGrabber.Object.InputAuthority)
			{
				if (newGrabber.Object.InputAuthority == Runner.LocalPlayer)
				{
					// Store data to handle the grab while the input authority transfer is pending
					m_willReceiveInputAuthority = true;
					m_inputAuthorityChangeRequestTime = Time.time;
					m_inputAuthorityChangeRequestTick = Runner.Tick;
					m_incomingGrabInfo = newGrabInfo;
					m_incomingGrabber = newGrabber;
				}

				// Transfering the input authority of the cube is in fact not strickly required here (as the object is fully simulated on all clients)
				if (Object.HasStateAuthority)
				{
					Object.AssignInputAuthority(newGrabber.Object.InputAuthority);
				}
			}

			m_cachedGrabbers[(newGrabber.Object.InputAuthority, newGrabber.Hand.Side)] = newGrabber;
		}

		public override void Ungrab(NetworkGrabber previousGrabber, NetworkGrabInfo newGrabInfo) { }

		#endregion

		#region NetworkBehaviour

		public override void Spawned()
		{
			base.Spawned();

			// When an object is grabbed by an user (non host), it is not yet input authority.
			//  But still, we already want to simulate physics as soon as possible (even before receiving input authority).
			//  Hence, the need to simulate on each client.
			//  Note that it can be replaced by extrapolation code (simulating the physics locally while waiting for input authority reception,
			//  disabling the NRB Render interpolation, and making sure the resim tick do not make the object advance too fast)
			Runner.SetIsSimulated(Object, true);

			m_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

			if (IsGrabbed)
			{
				DidGrab();
			}
		}

		private void Follow(Transform followedTransform, float elapsedTime, bool isColliding)
		{
			if (m_grabbable.PhysicsFollowMode == CustomPhysicsGrabbable.EInteractablePhysicsFollowMode.PID)
			{
				m_grabbable.PID.ErrorIntegration = PidErrorIntegration;
				m_grabbable.PID.LastError = PidLastError;
			}

			m_grabbable.Follow(followedTransform, elapsedTime, isColliding);

			if (m_grabbable.PhysicsFollowMode == CustomPhysicsGrabbable.EInteractablePhysicsFollowMode.PID)
			{
				PidErrorIntegration = m_grabbable.PID.ErrorIntegration;
				PidLastError = m_grabbable.PID.LastError;
			}
		}

		private NetworkGrabber GrabberForSideAndPlayer(PlayerRef player, ERigPart side)
		{
			return m_cachedGrabbers.ContainsKey((player, side)) ? m_cachedGrabbers[(player, side)] : null;
		}

		private NetworkGrabber GrabberForId(NetworkBehaviourId id)
		{
			return Runner.TryFindBehaviour(id, out NetworkGrabber grabber) ? grabber : null;
		}

		public override void FixedUpdateNetwork()
		{
			// ---- Handle waiting for input authority reception
			if (m_willReceiveInputAuthority && Object.HasInputAuthority)
			{
				// Authority received
				m_willReceiveInputAuthority = false;
			}

			if (m_willReceiveInputAuthority && Time.time - m_inputAuthorityChangeRequestTime > 1)
			{
				// Authority not received (quickly grabbed by someone else ?)
				m_willReceiveInputAuthority = false;
			}

			// ---- Reference previous state (up to date for host / input authority only - proxies grab info will always remain at the last confirmed value)
			bool wasGrabbed = DetailedGrabInfo.GrabbingUser != PlayerRef.None;
			NetworkBehaviourId previousGrabberId = DetailedGrabInfo.GrabInteractorId;

			// ---- Determine grabber/grab info for this tick
			bool isGrabbed = false;
			NetworkGrabInfo grabInfo = default;
			NetworkGrabber grabber = null;
			bool grabbingWhileNotYetInputAuthority = m_willReceiveInputAuthority && Runner.Tick > m_inputAuthorityChangeRequestTick;

			if (grabbingWhileNotYetInputAuthority)
			{
				// We are taking the input authority: we anticipate the grab before being able to read GetInput, by setting "manually" the grabber
				grabInfo = m_incomingGrabInfo;
				grabber = m_incomingGrabber;
			}
			else if (GetInput(out RigInput input))
			{
				if (input.LeftNetworkGrabInfo.GrabbedObjectId == Id)
				{
					isGrabbed = true;
					grabInfo = input.LeftNetworkGrabInfo;
					PlayerRef grabbingUser = Object.InputAuthority;
					grabber = GrabberForSideAndPlayer(grabbingUser, ERigPart.LeftController);
					m_previousGrabbingSide = ERigPart.LeftController;
				}
				else if (input.RightNetworkGrabInfo.GrabbedObjectId == Id)
				{
					isGrabbed = true;

					// one-hand grabbing only in this implementation
					grabInfo = input.RightNetworkGrabInfo;
					PlayerRef grabbingUser = Object.InputAuthority;
					grabber = GrabberForSideAndPlayer(grabbingUser, ERigPart.RightController);
					m_previousGrabbingSide = ERigPart.RightController;
				}
				else if (wasGrabbed && m_previousGrabbingSide != ERigPart.None)
				{
					grabInfo = m_previousGrabbingSide == ERigPart.LeftController ? input.LeftNetworkGrabInfo : input.RightNetworkGrabInfo;
				}
			}
			else
			{
				// Proxy
				isGrabbed = DetailedGrabInfo.GrabbingUser != PlayerRef.None;

				// one-hand grabbing only in this implementation
				grabInfo = DetailedGrabInfo.NetworkGrabInfo;

				if (isGrabbed)
				{
					grabber = GrabberForId(DetailedGrabInfo.GrabInteractorId);
				}
			}

			if (grabber != null)
			{
				// ---- Apply following move based on grabber/grabinfo
				if (isGrabbed)
				{
					m_grabbable.LocalPositionOffset = grabInfo.LocalPositionOffset;
					m_grabbable.LocalRotationOffset = grabInfo.LocalRotationOffset;
					Follow(grabber.transform, Runner.DeltaTime, IsColliding);
				}

				// ---- Store DetailedGrabInfo changes
				if (isGrabbed && (wasGrabbed == false || previousGrabberId != grabber.Id))
				{
					// New Grab
					// We do not store data as proxies, unless if we are waiting for the input authority
					if (!Object.IsProxy || grabbingWhileNotYetInputAuthority)
					{
						DetailedGrabInfo = new NetworkDetailedGrabInfo
						{
							GrabbingUser = grabber.Object.InputAuthority,
							GrabInteractorId = grabber.Id,
							NetworkGrabInfo = grabInfo
						};
					}
				}
			}

			if (wasGrabbed && isGrabbed == false)
			{
				// Ungrab
				// We do not store data as proxies, unless if we are waiting for the input authority
				if (Object.IsProxy == false || grabbingWhileNotYetInputAuthority)
				{
					DetailedGrabInfo = new NetworkDetailedGrabInfo
					{
						GrabbingUser = PlayerRef.None,
						GrabInteractorId = previousGrabberId,
						NetworkGrabInfo = grabInfo
					};
				}

				// Apply release velocity (the release timing is probably between tick, so we stored in the input the ungrab velocity to have sub-tick accuracy)
				m_grabbable.Rigidbody.velocity = grabInfo.UngrabVelocity;
				m_grabbable.Rigidbody.angularVelocity = grabInfo.UngrabAngularVelocity;
			}

			// ---- Trigger callbacks and release velocity
			// Callbacks are triggered only during forward tick to avoid triggering them several time due to resims.
			// If we are waiting for input authority, we do not check (and potentially trigger) the callbacks, as the DetailedGrabInfo will temporarily be erased by the server, and so that might trigger twice the callbacks later
			if (Runner.IsForward && grabbingWhileNotYetInputAuthority == false)
			{
				TriggerCallbacksOnForwardGrabbingChanges();
			}

			// ---- Consume the isColliding value: it will be reset in the next physics simulation (used in PID based moves)
			IsColliding = false;

			if (m_displayInRemoteTimeFrameWhenGrabbed && Runner.IsFirstTick && Runner.IsForward == false)
			{
				// Store the first resim ticks (latest confirmed from the host) for this simulation time, in order to compute a remote timeframe in render when grabbed by a remote hand
				m_lastLocalizations.Add(new Localization { Timestamp = Runner.SimulationTime, Position = transform.position, Rotation = transform.rotation });

				while (m_lastLocalizations.Count > 20)
				{
					m_lastLocalizations.RemoveAt(0);
				}
			}
		}

		private void TriggerCallbacksOnForwardGrabbingChanges()
		{
			foreach (string change in m_changeDetector.DetectChanges(this, out NetworkBehaviourBuffer previousBuffer, out NetworkBehaviourBuffer currentBuffer))
			{
				if (change == nameof(DetailedGrabInfo))
				{
					PropertyReader<NetworkDetailedGrabInfo> reader = GetPropertyReader<NetworkDetailedGrabInfo>(nameof(DetailedGrabInfo));
					(NetworkDetailedGrabInfo previousInfo, NetworkDetailedGrabInfo currentInfo) = reader.Read(previousBuffer, currentBuffer);
					bool wasGrabbingbeforeChange = previousInfo.GrabbingUser != PlayerRef.None;
					bool isGrabbingAfterChange = currentInfo.GrabbingUser != PlayerRef.None;

					if (wasGrabbingbeforeChange == false && isGrabbingAfterChange)
					{
						DidGrab();
					}

					if (wasGrabbingbeforeChange && isGrabbingAfterChange == false)
					{
						// If we are the player ungrabbing, and we displayed the ghost hands, we hide them
						bool wasGrabbingLocally = Runner.LocalPlayer == previousInfo.GrabbingUser;
						NetworkGrabber previousGrabber = GrabberForId(previousInfo.GrabInteractorId);

						// TODO rewrite this properly
						// if (wasGrabbingLocally && previousGrabber != null && previousGrabber.hand.LocalHardwareHand != null && previousGrabber.hand.LocalHardwareHand.localHandRepresentation != null)
						// {
						//     previousGrabber.hand.LocalHardwareHand.localHandRepresentation.DisplayMesh(false);
						// }

						DidUngrab(GrabberForId(previousInfo.GrabInteractorId));
					}
				}
			}
		}

		public override void Render()
		{
			base.Render();

			if (Object.InputAuthority != Runner.LocalPlayer)
			{
				// Allow to prevent local hardware grabbing of the same object
				m_grabbable.IsGrabbed = IsGrabbed;
			}

			if (CurrentGrabber != null && CurrentGrabber.HasInputAuthority == false && m_willReceiveInputAuthority == false)
			{
				/*
				 * We want to render in the remote time frame here.
				 * 
				 * As we forced the proxy simulation with SetIsSimulated (so that the physics is run locally), 
				 * the object is always simulated, so by default then the interpolation would be done between locally simulated ticks
				 * But while applying physics is important to handle collision with other simulated object, the position is not perfectly predicted,
				 * as the hand simulated position for the remote user is not moving, as it is probably, during those states
				 * 
				 * So, while the FUN position is used for local physics computation, for the final rendering of this object, we prefer to use the remote timeframe,
				 * which will interpolate between states where the hand were properly positioned to trigger the following
				 */
				Localization from = default;
				Localization to = default;

				bool fromFound = false;
				bool toFound = false;
				float targetTime = Runner.RemoteRenderTime;

				foreach (Localization loc in m_lastLocalizations)
				{
					if (loc.Timestamp < targetTime)
					{
						fromFound = true;
						from = loc;
					}
					else
					{
						to = loc;
						toFound = true;

						break;
					}
				}

				if (fromFound && toFound)
				{
					float remoteAlpha = Maths.Clamp01((targetTime - from.Timestamp) / (to.Timestamp - from.Timestamp));

					m_networkRigidbody.InterpolationTarget.transform.position = Vector3.Lerp(from.Position, to.Position, remoteAlpha);
					m_networkRigidbody.InterpolationTarget.transform.rotation = Quaternion.Slerp(from.Rotation, to.Rotation, remoteAlpha);
				}
			}

			// We don't place the hand on the object while we are waiting to receive the input authority as the timeframe transitioning might lead to erroneous hand repositioning
			// if (IsGrabbed && m_willReceiveInputAuthority == false)
			// {
			// 	Transform handVisual = CurrentGrabber.Hand.transform;
			// 	Transform grabbableVisual = m_networkRigidbody.InterpolationTarget.transform;
			//
			// 	// On remote user, we want the hand to stay glued to the object, even though the hand and the grabbed object may have various interpolation
			// 	handVisual.rotation = grabbableVisual.rotation * Quaternion.Inverse(m_grabbable.LocalRotationOffset);
			// 	handVisual.position = grabbableVisual.position - (handVisual.TransformPoint(m_grabbable.LocalPositionOffset) - handVisual.position);
			//
			// 	// Add pseudo haptic feedback if needed
			// 	ApplyPseudoHapticFeedback();
			// }
		}

		#endregion

		#region Collision handling and feedback

		private void OnCollisionStay(Collision collision)
		{
			if (Object)
			{
				IsColliding = true;
			}
		}

		// Display a "ghost" hand at the position of the real life hand when the distance between the representation (glued to the grabbed object, and driven by forces) and the IRL hand becomes too great
		//  Also apply a vibration proportionnal to this distance, so that the user can feel the dissonance between what they ask and what they can do
		private void ApplyPseudoHapticFeedback()
		{
			bool isLocalPlayerMostRecentGrabber = Runner.LocalPlayer == DetailedGrabInfo.GrabbingUser;

			if (m_pseudoHapticFeedbackConfiguration.EnablePseudoHapticFeedback && IsGrabbed && isLocalPlayerMostRecentGrabber)
			{
				//TODO handle rendering of local hardware hand along with haptic feedback for that hand

				// if (CurrentGrabber.hand.LocalHardwareHand.localHandRepresentation != null)
				// {
				//     var handVisual = CurrentGrabber.hand.transform;
				//     Vector3 dissonanceVector = handVisual.position - CurrentGrabber.hand.LocalHardwareHand.transform.position;
				//     float dissonance = dissonanceVector.magnitude;
				//     m_isPseudoHapticDisplayed = (IsColliding && dissonance > m_pseudoHapticFeedbackConfiguration.MinContactingDissonance);
				//     CurrentGrabber.hand.LocalHardwareHand.localHandRepresentation.DisplayMesh(m_isPseudoHapticDisplayed);
				//     if (m_isPseudoHapticDisplayed)
				//     {
				//         CurrentGrabber.hand.LocalHardwareHand.SendHapticImpulse(amplitude: Mathf.Clamp01(dissonance / m_pseudoHapticFeedbackConfiguration.MaxDissonanceDistance), duration: m_pseudoHapticFeedbackConfiguration.VibrationDuration);
				//     }
				// }
			}
		}

		#endregion
	}
}
