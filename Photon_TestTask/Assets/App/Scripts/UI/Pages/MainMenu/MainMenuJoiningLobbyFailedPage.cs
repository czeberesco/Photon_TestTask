using System.Collections;
using System.Threading.Tasks;
using Core.Interfaces;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace UI.Pages.MainMenu
{
	public class MainMenuJoiningLobbyFailedPage : UIPageBase
	{
		#region SerializeFields

		[SerializeField] private BaseButton m_tryAgainButton;

		#endregion

		#region PrivateFields

		[Inject] private INetworkRunnerProvider m_networkRunnerProvider;
		[Inject] private IGameLobbyHandler m_gameLobbyHandler;

		#endregion

		#region ProtectedMethods

		protected override void RegisterToEvents()
		{
			m_tryAgainButton.Clicked += OnTryAgainButtonPressed;
		}

		protected override void UnregisterFromEvents()
		{
			m_tryAgainButton.Clicked -= OnTryAgainButtonPressed;
		}

		#endregion

		#region PrivateMethods

		private void OnTryAgainButtonPressed()
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

		#region UnityMethods

		#endregion
	}
}
