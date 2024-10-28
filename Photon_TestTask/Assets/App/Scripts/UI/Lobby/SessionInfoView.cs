using Core;
using Fusion;
using TMPro;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace UI.Lobby
{
	public class SessionInfoView : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private TextMeshProUGUI m_sessionNameLabel;
		[SerializeField] private TextMeshProUGUI m_playersCountInfo;
		[SerializeField] private BaseButton m_joinButton;

		#endregion

		#region PrivateFields

		private SessionInfo m_sessionInfo;
		[Inject] private ConnectionHandler m_connectionHandler;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			RegisterToEvents();
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region PublicMethods

		public void Setup(SessionInfo sessionInfo)
		{
			m_sessionNameLabel.text = sessionInfo.Name;
			m_playersCountInfo.text = $"{sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";
			m_sessionInfo = sessionInfo;
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			m_joinButton.Clicked += JoinSessionInvoked;
		}

		private void UnregisterFromEvents()
		{
			m_joinButton.Clicked -= JoinSessionInvoked;
		}

		private void JoinSessionInvoked()
		{
			if (m_sessionInfo == null)
			{
				Debug.LogWarning($"Invalid {nameof(SessionInfo)} data. Failed to join session");

				return;
			}

			m_connectionHandler.JoinSession(m_sessionInfo);
		}

		#endregion

		#region NestedTypes

		public class Factory : PlaceholderFactory<SessionInfoView> { }

		#endregion
	}
}
