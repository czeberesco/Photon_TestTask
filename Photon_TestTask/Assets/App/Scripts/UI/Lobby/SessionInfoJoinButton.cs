using Core;
using Fusion;
using UnityEngine;
using Zenject;

namespace UI.Lobby
{
	public class SessionInfoJoinButton : ButtonBase
	{
		#region PrivateFields

		[Inject] private ConnectionHandler m_connectionHandler;

		private SessionInfo m_sessionInfo;

		#endregion

		#region PublicMethods

		public void Setup(SessionInfo sessionInfo)
		{
			m_sessionInfo = sessionInfo;
		}

		#endregion

		#region ProtectedMethods

		protected override void OnButtonClicked()
		{
			if (m_sessionInfo == null)
			{
				Debug.LogWarning($"Invalid {nameof(SessionInfo)} data. Failed to join session");

				return;
			}

			m_connectionHandler.JoinSession(m_sessionInfo);
		}

		#endregion
	}
}
