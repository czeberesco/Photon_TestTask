using Zenject;

namespace Core.Installers
{
	public class LoaderSceneInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			//This binding will automatically load main menu scene for project when initialized 
			Container.BindInterfacesAndSelfTo<GameLoader>().AsSingle().NonLazy();
		}
	}
}
