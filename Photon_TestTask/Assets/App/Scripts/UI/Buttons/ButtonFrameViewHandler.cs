using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
	public class ButtonFrameViewHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		#region SerializeFields

		[SerializeField] private GameObject m_frame;

		#endregion

		#region UnityMethods

		private void OnEnable()
		{
			m_frame.SetActive(false);
		}

		#endregion

		#region InterfaceImplementations

		public void OnPointerEnter(PointerEventData eventData)
		{
			m_frame.SetActive(true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			m_frame.SetActive(false);
		}

		#endregion
	}
}
