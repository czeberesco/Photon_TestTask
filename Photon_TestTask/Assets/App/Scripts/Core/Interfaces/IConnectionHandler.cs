using System.Threading.Tasks;
using Data;
using Fusion;

namespace Core.Interfaces
{
	public interface IConnectionHandler
	{
		#region PublicMethods

		public Task HostSession(GameLevelData gameLevelData, string sessionName);
		public Task JoinSession(SessionInfo sessionInfo);

		#endregion
	}
}
