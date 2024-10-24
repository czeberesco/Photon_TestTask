using UnityEngine;
using Utils;

namespace VR.Views
{
	[CreateAssetMenu(menuName = R.CREATE_APP_ASSET_MAIN_CATEGORY_NAME + nameof(RigElementViewSettings), fileName = nameof(RigElementViewSettings), order = 0)]
	public class RigElementViewSettings : ScriptableObject
	{
		#region Properties

		public Material VisibleMaterial => m_visibleMaterial;
		public Material GhostMaterial => m_ghostMaterial;

		#endregion

		#region SerializeFields

		[SerializeField] private Material m_visibleMaterial;
		[SerializeField] private Material m_ghostMaterial;

		#endregion
	}
}
