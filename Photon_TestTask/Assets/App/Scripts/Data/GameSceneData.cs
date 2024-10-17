using DefaultNamespace;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data
{
	[CreateAssetMenu(menuName = R.CREATE_APP_ASSET_MAIN_CATEGORY_NAME + nameof(GameSceneData), fileName = nameof(GameSceneData), order = 0)]
	public class GameSceneData : ScriptableObject
	{
		#region Properties

		public AssetReference SceneAssetReference => m_sceneAssetReference;

		#endregion

		#region SerializeFields

		[SerializeField] private AssetReference m_sceneAssetReference;

		#endregion
	}
}
