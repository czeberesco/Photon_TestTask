using System.Collections.Generic;
using Core.Interfaces;
using Data;
using Fusion;
using UI.Lobby;
using UnityEngine;
using Zenject;

namespace UI.Pages.MainMenu
{
	public class MainMenuLobbyPage : UIPageBase
	{
		#region SerializeFields

		[SerializeField] private BaseButton m_createSessionButton;
		[SerializeField] private SessionInfosUIContainer m_sessionInfosUIContainer;

		#endregion

		#region PrivateFields

		[Inject] private GameLevelDataCollection m_gameLevelDataCollection;
		[Inject] private IConnectionHandler m_connectionHandler;
		[Inject] private IGameLobbyHandler m_gameLobbyHandler;

		#endregion

		#region PublicMethods

		public override void Show()
		{
			base.Show();

			OnSessionListUpdated(m_gameLobbyHandler.GetCurrentSessions());
		}

		#endregion

		#region ProtectedMethods

		protected override void RegisterToEvents()
		{
			base.RegisterToEvents();

			m_createSessionButton.Clicked += OnCreateSessionButtonClicked;
			m_gameLobbyHandler.SessionListUpdated += OnSessionListUpdated;
		}

		protected override void UnregisterFromEvents()
		{
			base.UnregisterFromEvents();

			m_gameLobbyHandler.SessionListUpdated -= OnSessionListUpdated;
			m_createSessionButton.Clicked -= OnCreateSessionButtonClicked;
		}

		#endregion

		#region PrivateMethods

		private void OnCreateSessionButtonClicked()
		{
			GameLevelData gameLevelData = m_gameLevelDataCollection.Collection[Random.Range(0, m_gameLevelDataCollection.Collection.Count)];
			
			m_connectionHandler.HostSession(gameLevelData, $"Session_{Random.Range(0, 999999)}");
		}

		private void OnSessionListUpdated(List<SessionInfo> sessionInfoList)
		{
			m_sessionInfosUIContainer.RefreshSessionListView(sessionInfoList);
		}

		#endregion
	}
}
