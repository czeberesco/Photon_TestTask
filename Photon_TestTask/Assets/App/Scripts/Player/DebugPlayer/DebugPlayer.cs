using System.Collections;
using Core.Interfaces;
using Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace Player.DebugPlayer
{
	public class DebugPlayer : MonoBehaviour
	{
		#region Events

		[SerializeField] private InputActionReference m_leaveSessionButton;

		#endregion

		#region PrivateFields

		[Inject] private INetworkRunnerProvider m_networkRunnerProvider;
		[Inject] private GameLevelDataCollection m_gameLevelDataCollection;

		private bool m_isOccupied;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			RegisterToEvents();
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			m_leaveSessionButton.action.performed += OnLeaveSessionButtonPressed;
		}

		private void UnregisterFromEvents()
		{
			m_leaveSessionButton.action.performed -= OnLeaveSessionButtonPressed;
		}

		private void OnLeaveSessionButtonPressed(InputAction.CallbackContext callbackContext)
		{
			if (m_isOccupied)
			{
				return;
			}

			StartCoroutine(QuitSessionCoroutine());
		}

		private IEnumerator QuitSessionCoroutine()
		{
			m_isOccupied = true;

			yield return m_networkRunnerProvider.Reinitialize();

			AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_gameLevelDataCollection.LobbyLevel);

			yield return new WaitUntil(() => handle.IsDone);

			m_isOccupied = false;
		}

		#endregion
	}
}
