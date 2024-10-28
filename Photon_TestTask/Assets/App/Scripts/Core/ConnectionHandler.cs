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
		#region Events

		public event Action HostingSessionStarted;
		public event Action HostingSessionFailed;
		public event Action HostingSessionSuccess;
		public event Action JoiningSessionStarted;
		public event Action JoiningSessionFailed;
		public event Action JoiningSessionSuccess;

		#endregion

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

		public async Task HostSession(GameLevelData gameLevelData, string sessionName)
		{
			string scenePath = await AddressableUtils.GetScenePath(gameLevelData.SceneAssetReference);
			NetworkSceneInfo networkSceneInfo = AddressableUtils.GetNetworkSceneInfoFromPath(scenePath);

			var sessionProperties = new Dictionary<string, SessionProperty>
			{
				{ R.SESSION_PROPERTY_SCENE_PATH, scenePath }
			};

			StartGameArgs roomOptions = new()
			{
				SessionName = sessionName,
				PlayerCount = gameLevelData.MaxPlayers,
				Scene = networkSceneInfo,
				SceneManager = m_networkSceneManager,
				GameMode = GameMode.AutoHostOrClient,
				SessionProperties = sessionProperties
			};

			HostingSessionStarted?.Invoke();

			StartGameResult startGameResult = await m_networkRunnerProvider.Runner.StartGame(roomOptions);

			if (startGameResult.Ok)
			{
				HostingSessionSuccess?.Invoke();
				LogCurrentSessionProperties();
			}
			else
			{
				Debug.LogWarning(startGameResult.ErrorMessage);
				HostingSessionFailed?.Invoke();
			}
		}

		public async Task JoinSession(SessionInfo sessionInfo)
		{
			sessionInfo.Properties.TryGetValue(R.SESSION_PROPERTY_SCENE_PATH, out SessionProperty scenePath);

			if (scenePath == null || !scenePath.IsString || string.IsNullOrEmpty(scenePath))
			{
				Debug.LogWarning("Invalid scene path provided from selected session info, session will not start");
			}

			NetworkSceneInfo networkSceneInfo = AddressableUtils.GetNetworkSceneInfoFromPath(scenePath);

			StartGameArgs roomOptions = new()
			{
				SessionName = sessionInfo.Name,
				PlayerCount = sessionInfo.MaxPlayers,
				Scene = networkSceneInfo,
				SceneManager = m_networkSceneManager,
				GameMode = GameMode.Client,
				SessionProperties = new Dictionary<string, SessionProperty>(sessionInfo.Properties)
			};

			JoiningSessionStarted?.Invoke();

			StartGameResult startGameResult = await m_networkRunnerProvider.Runner.StartGame(roomOptions);

			if (startGameResult.Ok)
			{
				JoiningSessionSuccess?.Invoke();
				LogCurrentSessionProperties();
			}
			else
			{
				Debug.LogWarning(startGameResult.ErrorMessage);
				JoiningSessionFailed?.Invoke();
			}
		}

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
