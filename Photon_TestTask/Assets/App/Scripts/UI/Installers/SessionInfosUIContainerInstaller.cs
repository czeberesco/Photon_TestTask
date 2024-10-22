using UI.Lobby;
using UnityEngine;
using Zenject;

namespace UI.Installers
{
	public class SessionInfosUIContainerInstaller : MonoInstaller
	{
		#region SerializeFields

		[SerializeField] private SessionInfoView m_sessionInfoViewPrefab;

		#endregion

		#region PublicMethods

		public override void InstallBindings()
		{
			Container.BindFactory<SessionInfoView, SessionInfoView.Factory>().FromComponentInNewPrefab(m_sessionInfoViewPrefab);
		}

		#endregion
	}
}
