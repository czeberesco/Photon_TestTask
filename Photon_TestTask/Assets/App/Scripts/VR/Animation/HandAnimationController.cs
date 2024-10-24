using UnityEngine;

namespace VR.Animation
{
	public class HandAnimationController : MonoBehaviour
	{
		#region Statics

		private static readonly int m_gripHash = Animator.StringToHash("Grip");
		private static readonly int m_triggerHash = Animator.StringToHash("Trigger");

		#endregion

		#region SerializeFields

		[SerializeField] private Animator m_animator;
		[SerializeField] private HandAnimationSettings m_handAnimationSettings;

		#endregion

		#region PrivateFields

		private float m_targetGripValue;
		private float m_targetTriggerValue;

		private float m_gripAnimatedValue;
		private float m_triggerAnimatedValue;

		#endregion

		#region UnityMethods

		private void Update()
		{
			AnimateGrip();
			AnimateTrigger();
		}

		#endregion

		#region PublicMethods

		public void SetTargetGripValue(float normalizedValue)
		{
			normalizedValue = Mathf.Clamp01(normalizedValue);

			m_targetGripValue = normalizedValue;
		}

		public void SetTargetTriggerValue(float normalizedValue)
		{
			normalizedValue = Mathf.Clamp01(normalizedValue);

			m_targetTriggerValue = normalizedValue;
		}

		#endregion

		#region PrivateMethods

		private void AnimateGrip()
		{
			m_gripAnimatedValue = Mathf.MoveTowards(
				m_gripAnimatedValue,
				m_targetGripValue,
				Time.deltaTime * m_handAnimationSettings.AnimationSpeed
			);

			m_animator.SetFloat(m_gripHash, m_gripAnimatedValue);
		}

		private void AnimateTrigger()
		{
			m_triggerAnimatedValue = Mathf.MoveTowards(
				m_triggerAnimatedValue,
				m_targetTriggerValue,
				Time.deltaTime * m_handAnimationSettings.AnimationSpeed
			);

			m_animator.SetFloat(m_triggerHash, m_triggerAnimatedValue);
		}

		#endregion
	}
}
