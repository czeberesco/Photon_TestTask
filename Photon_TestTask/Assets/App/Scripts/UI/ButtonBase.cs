using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ButtonBase : MonoBehaviour
	{
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
			m_button.onClick.AddListener(OnButtonClicked);
		}

		protected virtual void UnregisterFromEvents()
		{
			m_button.onClick.RemoveListener(OnButtonClicked);
		}

		protected virtual void OnButtonClicked() { }

		#endregion
	}
}
