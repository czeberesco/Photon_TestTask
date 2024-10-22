using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;
using Zenject;

namespace UI.Lobby
{
	public class SessionInfosUIContainer : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private Transform m_sessionInfoViewsRoot;
		[SerializeField] private GameObject m_noSessionsAvailableLabel;

		#endregion

		#region PrivateFields

		[Inject] private IGameLobbyHandler m_gameLobbyHandler;
		[Inject] private SessionInfoView.Factory m_sessionInfoViewFactory;
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
				SessionInfoView sessionInfoView = m_sessionInfoViewFactory.Create();
				sessionInfoView.transform.parent = m_sessionInfoViewsRoot;
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
