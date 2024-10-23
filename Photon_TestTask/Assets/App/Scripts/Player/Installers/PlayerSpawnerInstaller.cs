using UnityEngine;
using Zenject;

namespace Player.Installers
{
	public class PlayerSpawnerInstaller : MonoInstaller
	{
		#region SerializeFields

		[SerializeField] private PlayerOfflineRig m_playerOfflineRig;

		#endregion

		#region PublicMethods

		public override void InstallBindings()
		{
			Container.BindFactory<PlayerOfflineRig, PlayerOfflineRig.Factory>().FromComponentInNewPrefab(m_playerOfflineRig);
		}

		#endregion
	}
}
