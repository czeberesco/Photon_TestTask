using Core.Interfaces;
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

		#endregion

		#region PublicMethods

		public override void InstallBindings()
		{
			Container.Bind<INetworkRunnerProvider>().FromInstance(m_networkRunnerProvider).AsSingle();
			Container.Bind<INetworkSceneManager>().FromInstance(m_networkSceneManager).AsSingle();
			Container.BindInterfacesAndSelfTo<ConnectionHandler>().AsSingle().NonLazy();
		}

		#endregion
	}
}
