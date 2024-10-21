using System;
using Data;

namespace Core.Interfaces
{
	public interface IGameLobbyHandler
	{
		#region Events

		public event Action<LobbyData> JoinedLobby;

		#endregion

		#region Properties

		public LobbyData CurrentLobby { get; }

		#endregion
	}
}
