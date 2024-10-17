using System;
using Core.Interfaces;
using Fusion;
using UnityEngine;

namespace Core
{
	public class NetworkRunnerProvider : MonoBehaviour, INetworkRunnerProvider
	{
		#region Events

		public event Action<NetworkRunner> RunnerInitialized;
		public event Action<NetworkRunner> RunnerWillBeDestroyed;

		#endregion

		#region Properties

		public NetworkRunner Runner
		{
			get
			{
				if (m_runner == null)
				{
					CreateRunner();
				}

				return m_runner;
			}
		}

		#endregion

		#region PrivateFields

		private NetworkRunner m_runner;

		#endregion

		#region UnityMethods

		public void OnDestroy()
		{
			DestroyRunner();
		}

		#endregion

		#region PrivateMethods

		private void CreateRunner()
		{
			Debug.Log($"Initializing {nameof(NetworkRunner)}");

			m_runner = gameObject.AddComponent<NetworkRunner>();

			Debug.Log($"{nameof(NetworkRunner)} initialized");

			RunnerInitialized?.Invoke(m_runner);
		}

		private void DestroyRunner()
		{
			Debug.Log($" Destroying {nameof(NetworkRunner)}");

			RunnerWillBeDestroyed?.Invoke(m_runner);
			Destroy(m_runner);

			Debug.Log($"{nameof(NetworkRunner)} destroyed");
		}

		#endregion
	}
}
