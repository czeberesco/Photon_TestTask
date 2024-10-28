using System;
using System.Collections;
using Fusion;

namespace Core.Interfaces
{
	public interface INetworkRunnerProvider
	{
		#region Events

		public event Action<NetworkRunner> RunnerInitialized;
		public event Action<NetworkRunner> RunnerWillBeDestroyed;

		#endregion

		#region Properties

		NetworkRunner Runner { get; }

		#endregion

		#region PublicMethods

		public IEnumerator Reinitialize();

		#endregion
	}
}
