using UnityEngine;
using VR.Animation;

namespace VR.Views
{
	public class HandView : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private HandAnimationController m_handAnimationController;

		#endregion

		#region PublicMethods

		public void UpdateHandViewInput(ControllerInput controllerInput)
		{
			m_handAnimationController.SetTargetGripValue(controllerInput.GripInputValue);
			m_handAnimationController.SetTargetTriggerValue(controllerInput.TriggerInputValue);
		}

		#endregion
	}
}
