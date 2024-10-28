using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Data
{
	[CreateAssetMenu(menuName = R.CREATE_APP_ASSET_MAIN_CATEGORY_NAME + nameof(GameLevelDataCollection), fileName = nameof(GameLevelDataCollection), order = 0)]
	public class GameLevelDataCollection : ScriptableObject
	{
		#region Properties

		public AssetReference LobbyLevel => m_lobbyLevel;
		public List<GameLevelData> Collection => m_collection;

		#endregion

		#region SerializeFields

		[SerializeField] private AssetReference m_lobbyLevel;
		[SerializeField] private List<GameLevelData> m_collection;

		#endregion
	}
}
