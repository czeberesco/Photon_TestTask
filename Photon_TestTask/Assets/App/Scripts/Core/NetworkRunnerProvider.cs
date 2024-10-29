using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Interfaces;
using Fusion;
using Fusion.Addons.Physics;
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

		public NetworkRunner Runner { get; private set; }

		#endregion

		#region SerializeFields

		#endregion

		#region UnityMethods

		private void Start()
		{
			CreateRunner();
		}

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

			GameObject newRunnerGameObject = new GameObject($"{nameof(NetworkRunner)}_(DynamicallySpawned)");
			newRunnerGameObject.transform.SetParent(transform);
			Runner = newRunnerGameObject.AddComponent<NetworkRunner>();
			newRunnerGameObject.AddComponent<RunnerSimulatePhysics3D>();

			Debug.Log($"{nameof(NetworkRunner)} initialized");

			RunnerInitialized?.Invoke(Runner);
		}

		private async Task DestroyRunner()
		{
			if (Runner == null)
			{
				return;
			}

			Debug.Log($" Destroying {nameof(NetworkRunner)}");

			RunnerWillBeDestroyed?.Invoke(Runner);
			await Runner.Shutdown();
			Destroy(Runner);

			Debug.Log($"{nameof(NetworkRunner)} destroyed");
		}

		#endregion
	}
}
