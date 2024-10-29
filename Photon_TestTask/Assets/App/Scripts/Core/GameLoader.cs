using System.Threading.Tasks;
using Data;
using Zenject;
#if !UNITY_EDITOR
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#endif

namespace Core
{
	public class GameLoader : IInitializable
	{
		#region PrivateFields

		private readonly GameLevelDataCollection m_gameLevelDataCollection;

		#endregion

		#region Constructors

		public GameLoader(GameLevelDataCollection gameLevelDataCollection)
		{
			m_gameLevelDataCollection = gameLevelDataCollection;
		}

		#endregion

		#region InterfaceImplementations

		public async void Initialize()
		{
			// Force load from main menu scene only in builds
#if !UNITY_EDITOR
			Debug.Log($"Loading {nameof(m_gameLevelDataCollection.LobbyLevel)}");

			AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_gameLevelDataCollection.LobbyLevel);

			await handle.Task;

			Debug.Log($"{nameof(m_gameLevelDataCollection.LobbyLevel)} loaded");
#else
			await Task.Delay(0);
#endif
		}

		#endregion
	}
}
