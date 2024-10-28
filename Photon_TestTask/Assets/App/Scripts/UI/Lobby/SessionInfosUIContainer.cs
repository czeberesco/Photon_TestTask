using System.Collections.Generic;
using Fusion;
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

		[Inject] private SessionInfoView.Factory m_sessionInfoViewFactory;
		private List<SessionInfoView> m_currentSessionViews = new();

		#endregion

		#region PublicMethods

		public void RefreshSessionListView(List<SessionInfo> sessionInfos)
		{
			foreach (SessionInfoView sessionInfoView in m_currentSessionViews)
			{
				Destroy(sessionInfoView.gameObject);
			}

			m_currentSessionViews.Clear();

			m_noSessionsAvailableLabel.SetActive(sessionInfos.Count == 0);

			foreach (SessionInfo sessionInfo in sessionInfos)
			{
				SessionInfoView sessionInfoView = m_sessionInfoViewFactory.Create();
				sessionInfoView.transform.parent = m_sessionInfoViewsRoot;
				sessionInfoView.Setup(sessionInfo);
				m_currentSessionViews.Add(sessionInfoView);
			}
		}

		#endregion
	}
}
