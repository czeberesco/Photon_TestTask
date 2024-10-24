using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;
#if !UNITY_EDITOR
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#endif

namespace Core
{
	public class GameLoader : IInitializable
	{
		#region PrivateFields

		private readonly AssetReference m_mainMenuSceneReference;

		#endregion

		#region Constructors

		public GameLoader(AssetReference mainMenuSceneReference)
		{
			m_mainMenuSceneReference = mainMenuSceneReference;
		}

		#endregion

		#region InterfaceImplementations

		public async void Initialize()
		{
			// Force load from main menu scene only in builds
#if !UNITY_EDITOR
			Debug.Log("Loading main menu scene");

			AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_mainMenuSceneReference);

			await handle.Task;

			Debug.Log("Main menu scene loaded");
#else
			await Task.Delay(0);
#endif
		}

		#endregion
	}
}
