using UI.Interfaces;
using UnityEngine;

namespace UI.Pages
{
	public class UIPageBase : MonoBehaviour, IUIPage
	{
		#region InterfaceImplementations

		public virtual void Show()
		{
			gameObject.SetActive(true);
			RegisterToEvents();
		}

		public virtual void Hide()
		{
			UnregisterFromEvents();
			gameObject.SetActive(false);
		}

		#endregion

		#region ProtectedMethods

		protected virtual void RegisterToEvents() { }

		protected virtual void UnregisterFromEvents() { }

		#endregion
	}
}
