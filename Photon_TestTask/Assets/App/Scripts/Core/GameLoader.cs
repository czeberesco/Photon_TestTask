using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

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
			Debug.Log("Loading main menu scene");

			AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_mainMenuSceneReference);

			await handle.Task;

			Debug.Log("Main menu scene loaded");
		}

		#endregion
	}
}
