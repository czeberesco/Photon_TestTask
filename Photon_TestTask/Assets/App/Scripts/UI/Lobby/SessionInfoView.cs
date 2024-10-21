using TMPro;
using UnityEngine;

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

		public void Setup(Fusion.SessionInfo sessionInfo)
		{
			m_sessionNameLabel.text = sessionInfo.Name;
			m_playersCountInfo.text = $"{sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";

			m_joinButton.Setup(sessionInfo);
		}

		#endregion
	}
}
