using UnityEngine;

namespace VR.Hands
{
	public abstract class HandAnimationControllerBase : MonoBehaviour
	{
		#region Statics

		private static readonly int m_gripHash = Animator.StringToHash("Grip");
		private static readonly int m_triggerHash = Animator.StringToHash("Trigger");

		#endregion

		#region SerializeFields

		[SerializeField] private Animator m_animator;

		#endregion

		#region PublicMethods

		public void SetGripValue(float normalizedValue)
		{
			normalizedValue = Mathf.Clamp01(normalizedValue);
			m_animator.SetFloat(m_gripHash, normalizedValue);
		}

		public void SetTriggerValue(float normalizedValue)
		{
			normalizedValue = Mathf.Clamp01(normalizedValue);
			m_animator.SetFloat(m_triggerHash, normalizedValue);
		}

		#endregion
	}
}
