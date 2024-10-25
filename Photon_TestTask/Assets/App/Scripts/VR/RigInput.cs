using Fusion;
using UnityEngine;
using VR.Interactions;
using VR.Interactions.Network;

namespace VR
{
	public struct RigInput : INetworkInput
	{
		#region PublicFields

		public Vector3 PlayAreaPosition;
		public Quaternion PlayAreaRotation;
		public Vector3 HeadPosition;
		public Quaternion HeadRotation;
		public Vector3 LeftHandPosition;
		public Quaternion LeftHandRotation;
		public Vector3 RightHandPosition;
		public Quaternion RightHandRotation;
		public ControllerInput LeftControllerInput;
		public ControllerInput RightControllerInput;
		public NetworkGrabInfo LeftNetworkGrabInfo;
		public NetworkGrabInfo RightNetworkGrabInfo;

		#endregion
	}
}
