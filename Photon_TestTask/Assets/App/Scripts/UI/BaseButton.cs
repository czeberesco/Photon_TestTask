using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class BaseButton : MonoBehaviour
	{
		#region Events

		public event Action Clicked;

		#endregion

		#region SerializeFields

		[SerializeField] private Button m_button;

		#endregion

		#region UnityMethods

		protected virtual void Awake()
		{
			RegisterToEvents();
		}

		protected virtual void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region ProtectedMethods

		protected virtual void RegisterToEvents()
		{
			m_button.onClick.AddListener(OnClicked);
		}

		protected virtual void UnregisterFromEvents()
		{
			m_button.onClick.RemoveListener(OnClicked);
		}

		protected virtual void OnClickedImpl() { }

		#endregion

		#region PrivateMethods

		private void OnClicked()
		{
			OnClickedImpl();
			Clicked?.Invoke();
		}

		#endregion
	}
}
