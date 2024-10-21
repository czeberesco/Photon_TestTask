using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Lobby
{
	public class SessionInfosUIContainer : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private SessionInfoView m_sessionInfoViewPrefab;
		[SerializeField] private Transform m_sessionInfoViewsRoot;
		[SerializeField] private GameObject m_noSessionsAvailableLabel;

		#endregion

		#region PrivateFields

		[Inject] private IGameLobbyHandler m_gameLobbyHandler;
		private List<SessionInfoView> m_currentSessionViews = new();

		#endregion

		#region UnityMethods

		private void OnEnable()
		{
			RegisterToEvents();
			OnSessionListUpdated(m_gameLobbyHandler.GetCurrentSessions());
		}

		private void OnDisable()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region PrivateMethods

		private void OnSessionListUpdated(List<Fusion.SessionInfo> sessionInfos)
		{
			foreach (SessionInfoView sessionInfoView in m_currentSessionViews)
			{
				Destroy(sessionInfoView.gameObject);
			}

			m_currentSessionViews.Clear();

			m_noSessionsAvailableLabel.SetActive(sessionInfos.Count == 0);

			foreach (Fusion.SessionInfo sessionInfo in sessionInfos)
			{
				SessionInfoView sessionInfoView = Instantiate(m_sessionInfoViewPrefab, m_sessionInfoViewsRoot);
				sessionInfoView.Setup(sessionInfo);
				m_currentSessionViews.Add(sessionInfoView);
			}
		}

		private void RegisterToEvents()
		{
			m_gameLobbyHandler.SessionListUpdated += OnSessionListUpdated;
		}

		private void UnregisterFromEvents()
		{
			m_gameLobbyHandler.SessionListUpdated -= OnSessionListUpdated;
		}

		#endregion
	}
}
