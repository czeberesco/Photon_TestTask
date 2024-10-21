using System;
using UnityEngine;

namespace VR
{
	[Serializable]
	public struct HapticSettings
	{
		#region PublicFields

		public bool Active;
		[Range(0.0f, 1.0f)] public float Intensity;
		public float Duration;

		#endregion
	}
}
