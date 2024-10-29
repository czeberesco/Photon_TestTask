using Data;
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
			Debug.Log($"Loading {nameof(m_gameLevelDataCollection.LobbyLevel)}");

			AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_gameLevelDataCollection.LobbyLevel);

			await handle.Task;

			Debug.Log($"{nameof(m_gameLevelDataCollection.LobbyLevel)} loaded");
		}

		#endregion
	}
}
