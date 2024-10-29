using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Fusion;

namespace Core.Interfaces
{
	public interface IGameLobbyHandler
	{
		#region Events

		event Action JoiningLobbyStarted;
		event Action<StartGameResult, LobbyData> JoinLobbySuccess;
		event Action<StartGameResult> JoinLobbyFailed;
		event Action<List<SessionInfo>> SessionListUpdated;

		#endregion

		#region Properties

		LobbyData CurrentLobby { get; }

		#endregion

		#region PublicMethods

		List<SessionInfo> GetCurrentSessions();
		Task JoinLobby();

		#endregion
	}
}
