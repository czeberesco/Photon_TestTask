using System;
using Fusion;

namespace VR
{
	[Serializable]
	public struct ControllerInput : INetworkInput
	{
		#region PublicFields

		public float GripInputValue;
		public float TriggerInputValue;

		#endregion
	}
}
