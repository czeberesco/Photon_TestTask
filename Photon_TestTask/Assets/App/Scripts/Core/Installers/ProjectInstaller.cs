using Core.Interfaces;
using Data;
using Fusion;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
	public class ProjectInstaller : MonoInstaller
	{
		#region SerializeFields

		[SerializeField] private NetworkRunnerProvider m_networkRunnerProvider;
		[SerializeField] private NetworkSceneManagerDefault m_networkSceneManager;
		[SerializeField] private PlayerPrefabsData m_playerPrefabsData;

		#endregion

		#region PublicMethods

		public override void InstallBindings()
		{
			Container.Bind<INetworkRunnerProvider>().FromInstance(m_networkRunnerProvider).AsSingle();
			Container.BindInterfacesAndSelfTo<NetworkRunnerEventsDispatcher>().AsSingle().NonLazy();
			Container.Bind<INetworkSceneManager>().FromInstance(m_networkSceneManager).AsSingle();
			Container.Bind<PlayerPrefabsData>().FromInstance(m_playerPrefabsData).AsSingle();
			Container.BindInterfacesAndSelfTo<ConnectionHandler>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<GameLobbyHandler>().AsSingle().NonLazy();
		}

		#endregion
	}
}
