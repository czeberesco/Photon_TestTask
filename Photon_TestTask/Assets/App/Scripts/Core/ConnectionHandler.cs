using System;
using Core.Interfaces;
using Fusion;
using UnityEngine;
using Zenject;

namespace Core
{
	public class ConnectionHandler : IInitializable, IDisposable
	{
		#region PrivateFields

		private readonly INetworkRunnerProvider m_networkRunnerProvider;
		private readonly INetworkSceneManager m_networkSceneManager;

		#endregion

		#region Constructors

		public ConnectionHandler(
			INetworkRunnerProvider networkRunnerProvider,
			INetworkSceneManager networkSceneManager
		)
		{
			m_networkRunnerProvider = networkRunnerProvider;
			m_networkSceneManager = networkSceneManager;
		}

		#endregion

		#region InterfaceImplementations

		public void Dispose()
		{
			Debug.Log($"{nameof(ConnectionHandler)} disposed");
		}

		public void Initialize()
		{
			Debug.Log($"{nameof(ConnectionHandler)} initialized");
		}

		#endregion
	}
}
