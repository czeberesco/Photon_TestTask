using Data;
using Player.Spawning;
using UnityEngine;
using VR.Offline;
using Zenject;

namespace Player.Installers
{
	public class PlayerSpawnerInstaller : MonoInstaller
	{
		#region SerializeFields

		[SerializeField] private PlayerPrefabsData m_playerPrefabsData;

		#endregion

		#region PublicMethods

		public override void InstallBindings()
		{
			Container.Bind<PlayerPrefabsData>().FromInstance(m_playerPrefabsData).AsSingle();
			Container.BindFactory<OfflineRig, OfflineRig.Factory>().FromComponentInNewPrefab(m_playerPrefabsData.OfflineRigPrefab);
			Container.BindFactory<AbstractSpawnStrategy, HostSpawner, HostSpawner.Factory>();
		}

		#endregion
	}
}
