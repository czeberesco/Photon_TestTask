using UnityEngine;
using Zenject;

namespace Player
{
	public class PlayerOfflineRig : MonoBehaviour
	{
		#region NestedTypes

		public class Factory : PlaceholderFactory<PlayerOfflineRig> { }

		#endregion
	}
}
