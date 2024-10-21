using Core.Interfaces;
using Data;
using UnityEngine;
using Zenject;

namespace UI.Lobby
{
	public class CreateSessionButton : ButtonBase
	{
		[Inject] private GameLevelDataCollection m_gameLevelDataCollection;
		[Inject] private IConnectionHandler m_connectionHandler;

		protected override void OnButtonClicked()
		{
			base.OnButtonClicked();

			GameLevelData gameLevelData = m_gameLevelDataCollection.Collection[Random.Range(0, m_gameLevelDataCollection.Collection.Count)];

			m_connectionHandler.HostSession(gameLevelData, $"Session_{Random.Range(0, 999999)}");
		}
	}
}
