using UnityEngine;

namespace UI
{
	public class UIPageBase : MonoBehaviour, IUIPage
	{
		#region InterfaceImplementations

		public virtual void Show()
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}

		#endregion
	}
}
