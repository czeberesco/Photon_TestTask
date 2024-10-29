using System.Collections.Generic;
using UnityEngine;

namespace UI.Pages
{
	public abstract class UIMenuBaseController : MonoBehaviour
	{
		#region Properties

		protected abstract List<UIPageBase> Pages { get; }

		#endregion

		#region ProtectedMethods

		protected void ShowPage(UIPageBase page)
		{
			HidePages();
			page.Show();
		}

		protected void HidePages()
		{
			foreach (UIPageBase page in Pages)
			{
				page.Hide();
			}
		}

		#endregion
	}
}
