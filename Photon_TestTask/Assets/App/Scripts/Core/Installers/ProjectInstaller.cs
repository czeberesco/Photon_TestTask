using Fusion;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
	public class ProjectInstaller : MonoInstaller
	{
		#region SerializeFields

		[SerializeField] private NetworkRunner m_networkRunner;

		#endregion

		#region PublicMethods

		public override void InstallBindings()
		{
			Container.Bind<NetworkRunner>().FromInstance(m_networkRunner).AsSingle();
		}

		#endregion
	}
}
