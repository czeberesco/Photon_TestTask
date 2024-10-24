using UnityEngine;
using VR.Enums;

namespace VR.Views
{
	public class RigElementVisibilityHandler : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private Renderer[] m_renderers;
		[SerializeField] private EElementVisibilityState m_visibilityState = EElementVisibilityState.Visible;
		[SerializeField] private RigElementViewSettings m_rigElementViewSettings;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			SetVisibilityState(m_visibilityState);
		}

		#endregion

		#region PublicMethods

		public void SetVisibilityState(EElementVisibilityState state)
		{
			switch (state)
			{
				case EElementVisibilityState.Visible:
					SetRenderersVisibilityState(true);
					SetMaterial(m_rigElementViewSettings.VisibleMaterial);

					break;
				case EElementVisibilityState.Ghost:
					SetRenderersVisibilityState(true);

					SetMaterial(m_rigElementViewSettings.GhostMaterial);

					break;
				case EElementVisibilityState.Invisible:
					SetRenderersVisibilityState(false);

					break;
			}

			m_visibilityState = state;
		}

		#endregion

		#region PrivateMethods

		private void SetMaterial(Material mat)
		{
			foreach (Renderer elementRenderer in m_renderers)
			{
				elementRenderer.material = mat;
			}
		}

		private void SetRenderersVisibilityState(bool isVisible)
		{
			foreach (Renderer elementRenderer in m_renderers)
			{
				elementRenderer.enabled = isVisible;
			}
		}

		#endregion
	}
}
