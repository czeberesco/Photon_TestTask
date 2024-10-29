using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Pages.HandMenu
{
	public class HandMenuController : UIMenuBaseController
	{
		#region Events

		[SerializeField] private InputActionReference m_menuButton;

		#endregion

		#region Properties

		protected override List<UIPageBase> Pages =>
			m_pages ??= new List<UIPageBase>
			{
				m_mainPage
			};

		#endregion

		#region SerializeFields

		[SerializeField] private UIPageBase m_mainPage;

		#endregion

		#region PrivateFields

		private List<UIPageBase> m_pages;

		#endregion

		#region PrivateMethods

		private void OnMenuButtonPressed(InputAction.CallbackContext callbackContext)
		{
			ShowPage(m_mainPage);
		}

		private void OnMenuButtonReleased(InputAction.CallbackContext callbackContext)
		{
			HidePages();
		}

		private void RegisterToEvents()
		{
			m_menuButton.action.started += OnMenuButtonPressed;
			m_menuButton.action.canceled += OnMenuButtonReleased;
		}

		private void UnregisterFromEvents()
		{
			m_menuButton.action.canceled -= OnMenuButtonReleased;
			m_menuButton.action.started -= OnMenuButtonPressed;
		}

		#endregion

		#region UnityMethods

		private void Awake()
		{
			RegisterToEvents();
			HidePages();
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion
	}
}
