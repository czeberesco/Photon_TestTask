using System;
using System.Threading.Tasks;
using Data;
using Fusion;

namespace Core.Interfaces
{
	public interface IConnectionHandler
	{
		#region Events

		public event Action HostingSessionStarted;
		public event Action HostingSessionFailed;
		public event Action HostingSessionSuccess;

		public event Action JoiningSessionStarted;
		public event Action JoiningSessionFailed;
		public event Action JoiningSessionSuccess;

		#endregion

		#region PublicMethods

		public Task HostSession(GameLevelData gameLevelData, string sessionName);
		public Task JoinSession(SessionInfo sessionInfo);

		#endregion
	}
}
