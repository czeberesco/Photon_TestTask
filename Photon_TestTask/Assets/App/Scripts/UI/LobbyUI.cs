using System;
using Core.Interfaces;
using Data;
using UnityEngine;
using Zenject;

namespace UI
{
	public class LobbyUI : MonoBehaviour
	{
		#region PrivateFields

		[Inject] private IGameLobbyHandler m_gameLobbyHandler;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			m_gameLobbyHandler.JoinedLobby += OnJoinedLobby;
		}

		#endregion

		#region PrivateMethods

		private void OnJoinedLobby(LobbyData lobbyData)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
