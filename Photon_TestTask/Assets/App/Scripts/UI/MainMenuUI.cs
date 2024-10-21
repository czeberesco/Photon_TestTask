using System.Collections.Generic;
using Core.Interfaces;
using Data;
using Fusion;
using UnityEngine;
using Zenject;

namespace UI
{
	public class MainMenuUI : MonoBehaviour
	{
		#region Properties

		private List<UIPageBase> Pages =>
			m_pages ??= new List<UIPageBase>
			{
				m_joiningLobbyPage,
				m_joiningLobbyFailedPage,
				m_lobbyPage
			};

		#endregion

		#region SerializeFields

		[SerializeField] private UIPageBase m_joiningLobbyPage;
		[SerializeField] private UIPageBase m_joiningLobbyFailedPage;
		[SerializeField] private UIPageBase m_lobbyPage;

		#endregion

		#region PrivateFields

		[Inject] private IGameLobbyHandler m_gameLobbyHandler;
		private List<UIPageBase> m_pages;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			RegisterToEvents();

			HidePages();

			if (m_gameLobbyHandler.CurrentLobby.IsValid())
			{
				m_lobbyPage.Show();
			}
			else
			{
				m_joiningLobbyPage.Show();
			}
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region PrivateMethods

		private void OnJoinLobbySuccess(StartGameResult startGameResult, LobbyData lobbyData)
		{
			HidePages();
			m_lobbyPage.Show();
		}

		private void OnJoinLobbyFailed(StartGameResult startGameResult)
		{
			HidePages();
			m_joiningLobbyFailedPage.Show();
		}

		private void RegisterToEvents()
		{
			m_gameLobbyHandler.JoinLobbySuccess += OnJoinLobbySuccess;
			m_gameLobbyHandler.JoinLobbyFailed += OnJoinLobbyFailed;
		}

		private void UnregisterFromEvents()
		{
			if (m_gameLobbyHandler == null)
			{
				return;
			}

			m_gameLobbyHandler.JoinLobbyFailed -= OnJoinLobbyFailed;
			m_gameLobbyHandler.JoinLobbySuccess -= OnJoinLobbySuccess;
		}

		private void HidePages()
		{
			foreach (UIPageBase page in Pages)
			{
				page.Hide();
			}
		}

		#endregion
	}
}
