using DefaultNamespace;
using Fusion;
using UnityEngine;

namespace Data
{
	[CreateAssetMenu(menuName = R.CREATE_APP_ASSET_MAIN_CATEGORY_NAME + nameof(PlayerPrefabsData), fileName = nameof(PlayerPrefabsData), order = 0)]
	public class PlayerPrefabsData : ScriptableObject
	{
		#region Properties

		public NetworkObject PlayerPrefab => m_playerPrefab;

		#endregion

		#region SerializeFields

		[SerializeField] private NetworkObject m_playerPrefab;

		#endregion
	}
}
