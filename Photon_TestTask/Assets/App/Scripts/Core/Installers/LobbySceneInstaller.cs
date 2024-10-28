using Zenject;

namespace Core.Installers
{
	public class LobbySceneInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<GameLobbyHandler>().AsSingle().NonLazy();
		}
	}
}
