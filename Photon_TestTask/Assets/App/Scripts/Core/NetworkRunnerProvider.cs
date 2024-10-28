using System;
using System.Collections;
using System.Threading.Tasks;
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

		public NetworkRunner Runner => m_runner;

		#endregion

		#region SerializeFields

		[SerializeField] private NetworkRunner m_runner;

		#endregion

		#region UnityMethods

		private void OnDestroy()
		{
			DestroyRunner();
		}

		#endregion

		#region InterfaceImplementations

		public IEnumerator Reinitialize()
		{
			Task task = DestroyRunner();

			yield return new WaitUntil(() => task.IsCompleted);

			CreateRunner();
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

		private async Task DestroyRunner()
		{
			if (m_runner == null)
			{
				return;
			}

			Debug.Log($" Destroying {nameof(NetworkRunner)}");

			RunnerWillBeDestroyed?.Invoke(m_runner);
			await m_runner.Shutdown(false);
			Destroy(m_runner);

			Debug.Log($"{nameof(NetworkRunner)} destroyed");
		}

		#endregion
	}
}
