using UnityEngine;
using Utils;

namespace VR.Animation
{
	[CreateAssetMenu(menuName = R.CREATE_APP_ASSET_MAIN_CATEGORY_NAME + nameof(HandAnimationSettings), fileName = nameof(HandAnimationSettings), order = 0)]
	public class HandAnimationSettings : ScriptableObject
	{
		#region Properties

		public float AnimationSpeed => m_animationSpeed;

		#endregion

		#region SerializeFields

		[SerializeField] private float m_animationSpeed = 10.0f;

		#endregion
	}
}
