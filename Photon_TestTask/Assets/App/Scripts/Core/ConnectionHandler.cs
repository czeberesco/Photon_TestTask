using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Data;
using Fusion;
using UnityEngine;
using Utils;
using Zenject;

namespace Core
{
	public class ConnectionHandler : IConnectionHandler, IInitializable, IDisposable
	{
		#region PrivateFields

		private readonly INetworkRunnerProvider m_networkRunnerProvider;
		private readonly INetworkSceneManager m_networkSceneManager;

		#endregion

		#region Constructors

		public ConnectionHandler(
			INetworkRunnerProvider networkRunnerProvider,
			INetworkSceneManager networkSceneManager
		)
		{
			m_networkRunnerProvider = networkRunnerProvider;
			m_networkSceneManager = networkSceneManager;
		}

		#endregion

		#region InterfaceImplementations

		public void Dispose()
		{
			Debug.Log($"{nameof(ConnectionHandler)} disposed");
		}

		public void Initialize()
		{
			Debug.Log($"{nameof(ConnectionHandler)} initialized");
		}

		#endregion

		#region PrivateMethods

		public async Task HostSession(GameLevelData gameLevelData, string sessionName)
		{
			string scenePath = await AddressableUtils.GetScenePath(gameLevelData.SceneAssetReference);
			NetworkSceneInfo networkSceneInfo = AddressableUtils.GetNetworkSceneInfoFromPath(scenePath);

			var sessionProperties = new Dictionary<string, SessionProperty>
			{
				{ R.SESSION_PROPERTY_SCENE_PATH, scenePath }
			};

			var roomOptions = new StartGameArgs
			{
				SessionName = sessionName,
				PlayerCount = gameLevelData.MaxPlayers,
				Scene = networkSceneInfo,
				SceneManager = m_networkSceneManager,
				GameMode = GameMode.AutoHostOrClient,
				SessionProperties = sessionProperties
			};

			await m_networkRunnerProvider.Runner.StartGame(roomOptions);

			LogCurrentSessionProperties();
		}

		public async Task JoinSession(SessionInfo sessionInfo)
		{
			sessionInfo.Properties.TryGetValue(R.SESSION_PROPERTY_SCENE_PATH, out SessionProperty scenePath);
		
			if (scenePath == null || !scenePath.IsString || string.IsNullOrEmpty(scenePath))
			{
				Debug.LogWarning("Invalid scene path provided from selected session info, session will not start");
			}
		
			NetworkSceneInfo networkSceneInfo = AddressableUtils.GetNetworkSceneInfoFromPath(scenePath);
		
			var roomOptions = new StartGameArgs
			{
				SessionName = sessionInfo.Name,
				PlayerCount = sessionInfo.MaxPlayers,
				Scene = networkSceneInfo,
				SceneManager = m_networkSceneManager,
				GameMode = GameMode.Client,
				SessionProperties = new Dictionary<string, SessionProperty>(sessionInfo.Properties)
			};
		
			await m_networkRunnerProvider.Runner.StartGame(roomOptions);
		
			LogCurrentSessionProperties();
		}

		private void LogCurrentSessionProperties()
		{
			NetworkRunner runner = m_networkRunnerProvider.Runner;

			string prop = "";

			if (runner.SessionInfo.Properties != null && runner.SessionInfo.Properties.Count > 0)
			{
				prop = "SessionProperties: ";

				foreach (KeyValuePair<string, SessionProperty> p in runner.SessionInfo.Properties)
				{
					prop += $" ({p.Key}={p.Value.PropertyValue}) ";
				}
			}

			Debug.Log($"Session info: Room name {runner.SessionInfo.Name}. Region: {runner.SessionInfo.Region}. {prop}");
		}

		#endregion
	}
}
