using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Data
{
	[CreateAssetMenu(menuName = R.CREATE_APP_ASSET_MAIN_CATEGORY_NAME + nameof(GameLevelData), fileName = nameof(GameLevelData), order = 0)]
	public class GameLevelData : ScriptableObject
	{
		#region Properties

		public AssetReference SceneAssetReference => m_sceneAssetReference;

		public int MaxPlayers => m_maxPlayers;

		public string LevelName => m_levelName;

		#endregion

		#region SerializeFields

		[SerializeField] private AssetReference m_sceneAssetReference;
		[SerializeField] private int m_maxPlayers = 4;
		[SerializeField] private string m_levelName;

		#endregion
	}
}
