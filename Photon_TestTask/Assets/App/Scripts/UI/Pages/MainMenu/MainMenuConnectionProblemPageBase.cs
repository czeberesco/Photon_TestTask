using System.Collections;
using System.Threading.Tasks;
using Core.Interfaces;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace UI.Pages.MainMenu
{
	public class MainMenuConnectionProblemPageBase : UIPageBase
	{
		#region SerializeFields

		[SerializeField] private BaseButton m_restartLobbyButton;

		#endregion

		#region PrivateFields

		[Inject] private INetworkRunnerProvider m_networkRunnerProvider;
		[Inject] private IGameLobbyHandler m_gameLobbyHandler;

		#endregion

		#region ProtectedMethods

		protected override void RegisterToEvents()
		{
			m_restartLobbyButton.Clicked += OnRestartLobbyButtonPressed;
		}

		protected override void UnregisterFromEvents()
		{
			m_restartLobbyButton.Clicked -= OnRestartLobbyButtonPressed;
		}

		#endregion

		#region PrivateMethods

		private void OnRestartLobbyButtonPressed()
		{
			StartCoroutine(TryAgainToConnectLobby());
		}

		private IEnumerator TryAgainToConnectLobby()
		{
			yield return m_networkRunnerProvider.Reinitialize();

			Task rejoin = m_gameLobbyHandler.JoinLobby();

			yield return new WaitUntil(() => rejoin.IsCompleted);
		}

		#endregion
	}
}
