using System.Collections.Generic;
using Core.Interfaces;
using Data;
using Fusion;
using UnityEngine;
using Zenject;

namespace UI.Pages.LobbyMenu
{
	public class LobbyMenuController : UIMenuBaseController
	{
		#region Properties

		protected override List<UIPageBase> Pages =>
			m_pages ??= new List<UIPageBase>
			{
				m_joiningLobbyPage,
				m_joiningLobbyFailedPage,
				m_lobbyPage,
				m_hostingSessionPage,
				m_joiningSessionPage,
				m_startSessionFailedPage
			};

		#endregion

		#region SerializeFields

		[SerializeField] private UIPageBase m_joiningLobbyPage;
		[SerializeField] private UIPageBase m_joiningLobbyFailedPage;
		[SerializeField] private UIPageBase m_lobbyPage;
		[SerializeField] private UIPageBase m_hostingSessionPage;
		[SerializeField] private UIPageBase m_joiningSessionPage;
		[SerializeField] private UIPageBase m_startSessionFailedPage;

		#endregion

		#region PrivateFields

		[Inject] private IGameLobbyHandler m_gameLobbyHandler;
		[Inject] private IConnectionHandler m_connectionHandler;
		private List<UIPageBase> m_pages;

		#endregion

		#region PrivateMethods

		private void OnJoiningLobbyStarted()
		{
			ShowPage(m_joiningLobbyPage);
		}

		private void OnJoinLobbySuccess(StartGameResult startGameResult, LobbyData lobbyData)
		{
			ShowPage(m_lobbyPage);
		}

		private void OnJoinLobbyFailed(StartGameResult startGameResult)
		{
			ShowPage(m_joiningLobbyFailedPage);
		}

		private void OnHostingSessionStarted()
		{
			ShowPage(m_hostingSessionPage);
		}

		private void OnHostingSessionFailed()
		{
			ShowPage(m_startSessionFailedPage);
		}

		private void OnJoiningSessionStarted()
		{
			ShowPage(m_joiningSessionPage);
		}

		private void OnJoiningSessionFailed()
		{
			ShowPage(m_startSessionFailedPage);
		}

		private void RegisterToEvents()
		{
			m_gameLobbyHandler.JoiningLobbyStarted += OnJoiningLobbyStarted;
			m_gameLobbyHandler.JoinLobbySuccess += OnJoinLobbySuccess;
			m_gameLobbyHandler.JoinLobbyFailed += OnJoinLobbyFailed;
			m_connectionHandler.HostingSessionStarted += OnHostingSessionStarted;
			m_connectionHandler.HostingSessionFailed += OnHostingSessionFailed;
			m_connectionHandler.JoiningSessionStarted += OnJoiningSessionStarted;
			m_connectionHandler.JoiningSessionFailed += OnJoiningSessionFailed;
		}

		private void UnregisterFromEvents()
		{
			if (m_connectionHandler != null)
			{
				m_connectionHandler.JoiningSessionFailed -= OnJoiningSessionFailed;
				m_connectionHandler.JoiningSessionStarted -= OnJoiningSessionStarted;
				m_connectionHandler.HostingSessionFailed -= OnHostingSessionFailed;
				m_connectionHandler.HostingSessionStarted -= OnHostingSessionStarted;
			}

			if (m_gameLobbyHandler != null)
			{
				m_gameLobbyHandler.JoinLobbyFailed -= OnJoinLobbyFailed;
				m_gameLobbyHandler.JoinLobbySuccess -= OnJoinLobbySuccess;
				m_gameLobbyHandler.JoiningLobbyStarted -= OnJoiningLobbyStarted;
			}
		}

		#endregion

		#region UnityMethods

		private void Awake()
		{
			RegisterToEvents();

			ShowPage(m_gameLobbyHandler.CurrentLobby.IsValid() ? m_lobbyPage : m_joiningLobbyPage);
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion
	}
}
