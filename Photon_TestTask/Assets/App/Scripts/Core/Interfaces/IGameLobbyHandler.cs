using System;
using Data;
using Fusion;

namespace Core.Interfaces
{
	public interface IGameLobbyHandler
	{
		#region Events

		public event Action<StartGameResult, LobbyData> JoinLobbySuccess;
		public event Action<StartGameResult> JoinLobbyFailed;

		#endregion

		#region Properties

		public LobbyData CurrentLobby { get; }

		#endregion
	}
}
