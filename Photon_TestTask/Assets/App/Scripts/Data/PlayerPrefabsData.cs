using Fusion;
using Player;
using UnityEngine;
using Utils;

namespace Data
{
	[CreateAssetMenu(menuName = R.CREATE_APP_ASSET_MAIN_CATEGORY_NAME + nameof(PlayerPrefabsData), fileName = nameof(PlayerPrefabsData), order = 0)]
	public class PlayerPrefabsData : ScriptableObject
	{
		#region Properties

		public NetworkObject PlayerNetworkPrefab => m_playerNetworkPrefab;

		public PlayerOfflineRig PlayerOfflineRigPrefab => m_playerOfflineRigPrefab;

		#endregion

		#region SerializeFields

		[SerializeField] private NetworkObject m_playerNetworkPrefab;
		[SerializeField] private PlayerOfflineRig m_playerOfflineRigPrefab;

		#endregion
	}
}
