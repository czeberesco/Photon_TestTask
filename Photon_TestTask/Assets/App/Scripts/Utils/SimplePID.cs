using System;
using UnityEngine;

namespace Utils
{
	/***
 * 
var error = targetPosition - grabbableTransform->Position;
var command = pidState.UpdateCommand(error, f.DeltaTime, config.pidSettings, ignoreIntegration: config.ignorePidIntegrationWhileColliding && isColliding);
var impulse = FPVector3.ClampMagnitude(config.commandScale * command, config.maxCommandMagnitude);
grabbableBody->AddLinearImpulse(impulse);
grabbableTransform->Rotation = targetRotation;
 * 
 * */

	[Serializable]
	public struct PIDSettings
	{
		#region PublicFields

		public float ProportionalGain;
		public float IntegralGain;
		public float DerivativeGain;
		public float MaxIntegrationMagnitude;

		#endregion
	}

	[Serializable]
	public struct PIDState
	{
		#region PublicFields

		public PIDSettings PidSettings;
		public Vector3 ErrorIntegration;
		public Vector3 LastError;

		#endregion

		#region PrivateFields

		private Vector3 m_lastPosition;
		private bool m_derivateInitialized;

		#endregion

		#region PublicMethods

		// For explanation about PID controllers, see https://www.youtube.com/watch?v=y3K6FUgrgXw
		public Vector3 UpdateCommand(Vector3 error, float dt, bool ignoreIntegration = false)
		{
			// P
			Vector3 p = PidSettings.ProportionalGain * error;

			// I
			Vector3 i = Vector3.zero;

			if (ignoreIntegration == false)
			{
				Vector3 newErrorIntegration = ErrorIntegration + dt * error;
				ErrorIntegration = Vector3.ClampMagnitude(newErrorIntegration, PidSettings.MaxIntegrationMagnitude);
				i = PidSettings.IntegralGain * ErrorIntegration;
			}

			// D
			Vector3 d = Vector3.zero;

			if (m_derivateInitialized)
			{
				Vector3 errorDerivate = (error - LastError) / dt;
				d = PidSettings.DerivativeGain * errorDerivate;
			}

			// Update for next controller update
			LastError = error;
			m_derivateInitialized = true;

			// Command
			return p + i + d;
		}

		public void Reset()
		{
			Debug.LogError("Reset");
			LastError = Vector3.zero;
			m_derivateInitialized = false;
			ErrorIntegration = Vector3.zero;
		}

		#endregion
	}
}
