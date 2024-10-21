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
	public class Test : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private GameLevelData m_gameLevelData;

		#endregion

		#region PrivateFields

		[Inject] private INetworkSceneManager m_networkSceneManager;
		[Inject] private INetworkRunnerProvider m_networkRunnerProvider;

		#endregion

		#region PrivateMethods

		private async Task LoadGameScene()
		{
			NetworkRunner runner = m_networkRunnerProvider.Runner;

			NetworkSceneInfo networkSceneInfo = await AddressableUtils.GetNetworkSceneInfoFromAssetReference(m_gameLevelData.SceneAssetReference);

			var roomOptions = new StartGameArgs
			{
				SessionName = "session1",
				PlayerCount = 10,
				Scene = networkSceneInfo,
				SceneManager = m_networkSceneManager,
				GameMode = GameMode.Shared
			};

			await m_networkRunnerProvider.Runner.StartGame(roomOptions);

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
