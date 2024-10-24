using Player;
using Player.Interfaces;
using VR.Offline;
using Zenject;

namespace Core.Installers
{
	public class GameSceneInstaller : MonoInstaller
	{
		#region PublicMethods

		public override void InstallBindings()
		{
			Container.Bind<IPlayerOfflineRigProvider>().To<OfflineRigProvider>().AsSingle();
		}

		#endregion
	}
}
