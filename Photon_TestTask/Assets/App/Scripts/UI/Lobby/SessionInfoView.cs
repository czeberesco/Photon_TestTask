using Fusion;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Lobby
{
	public class SessionInfoView : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private TextMeshProUGUI m_sessionNameLabel;
		[SerializeField] private TextMeshProUGUI m_playersCountInfo;
		[SerializeField] private SessionInfoJoinButton m_joinButton;

		#endregion

		#region PublicMethods

		public void Setup(SessionInfo sessionInfo)
		{
			m_sessionNameLabel.text = sessionInfo.Name;
			m_playersCountInfo.text = $"{sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";

			m_joinButton.Setup(sessionInfo);
		}

		#endregion

		#region NestedTypes

		public class Factory : PlaceholderFactory<SessionInfoView> { }

		#endregion
	}
}
