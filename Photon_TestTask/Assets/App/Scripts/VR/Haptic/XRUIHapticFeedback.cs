using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace VR.Haptic
{
	public class XRUIHapticFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		#region Properties

		private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;

		#endregion

		#region SerializeFields

		[SerializeField] private HapticSettings m_pointerEnter;
		[SerializeField] private HapticSettings m_pointerExit;
		[SerializeField] private HapticSettings m_pointerDown;
		[SerializeField] private HapticSettings m_pointerUp;

		#endregion

		#region InterfaceImplementations

		public void OnPointerDown(PointerEventData eventData)
		{
			HandleHapticFeedback(eventData, m_pointerDown);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			HandleHapticFeedback(eventData, m_pointerEnter);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			HandleHapticFeedback(eventData, m_pointerExit);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			HandleHapticFeedback(eventData, m_pointerUp);
		}

		#endregion

		#region PrivateMethods

		private void HandleHapticFeedback(PointerEventData pointerEventData, HapticSettings hapticSettings)
		{
			if (!hapticSettings.Active)
			{
				return;
			}

			XRBaseInputInteractor interactor = InputModule.GetInteractor(pointerEventData.pointerId) as XRBaseInputInteractor;

			if (interactor == null)
			{
				return;
			}

			interactor.SendHapticImpulse(hapticSettings.Intensity, hapticSettings.Duration);
		}

		#endregion
	}
}
