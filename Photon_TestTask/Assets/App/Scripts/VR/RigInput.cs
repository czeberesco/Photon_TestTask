using Fusion;
using UnityEngine;

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

		#endregion
	}
}
