using System.Collections;
using Core.Interfaces;
using Data;
using UI.Buttons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;

namespace UI.Pages.HandMenu
{
	public class HandMenuMainPage : UIPageBase
	{
		#region SerializeFields

		[SerializeField] private BaseButton m_quitSessionButton;

		#endregion

		#region PrivateFields

		[Inject] private INetworkRunnerProvider m_networkRunnerProvider;
		[Inject] private GameLevelDataCollection m_gameLevelDataCollection;

		#endregion

		#region ProtectedMethods

		protected override void RegisterToEvents()
		{
			base.RegisterToEvents();

			m_quitSessionButton.Clicked += QuitSession;
		}

		protected override void UnregisterFromEvents()
		{
			base.UnregisterFromEvents();

			m_quitSessionButton.Clicked -= QuitSession;
		}

		#endregion

		#region PrivateMethods

		private void QuitSession()
		{
			StartCoroutine(QuitSessionCoroutine());
		}

		private IEnumerator QuitSessionCoroutine()
		{
			m_quitSessionButton.SetInteractable(false);

			yield return m_networkRunnerProvider.Reinitialize();

			AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_gameLevelDataCollection.LobbyLevel);

			yield return new WaitUntil(() => handle.IsDone);

			m_quitSessionButton.SetInteractable(true);
		}

		#endregion
	}
}
