using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.DebugUI
{
	public class DebugUISetup : MonoBehaviour
	{
		#region Events

		[SerializeField] private InputActionReference m_toggleUIRenderModeActionReference;

		#endregion

		#region SerializeFields

		[SerializeField] private Canvas m_canvas;

		#endregion

		#region PrivateFields

		private Vector3 m_initWorldspacePosition;
		private Quaternion m_initWorldspaceRotation;
		private Vector3 m_initWorldspaceScale;
		private Vector2 m_initSize;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			RegisterToEvents();
			SaveInitValues();
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region PrivateMethods

		private void OnToggleUIRenderMode(InputAction.CallbackContext callbackContext)
		{
			switch (m_canvas.renderMode)
			{
				case RenderMode.ScreenSpaceOverlay:
				case RenderMode.ScreenSpaceCamera:
					m_canvas.renderMode = RenderMode.WorldSpace;
					RestoreInitValues();

					break;
				case RenderMode.WorldSpace:
				default:
					m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;

					break;
			}
		}

		private void SaveInitValues()
		{
			if (m_canvas.renderMode == RenderMode.WorldSpace)
			{
				RectTransform rectTransform = m_canvas.GetComponent<RectTransform>();
				m_initWorldspacePosition = rectTransform.position;
				m_initWorldspaceRotation = rectTransform.rotation;
				m_initWorldspaceScale = rectTransform.localScale;

				Rect rect = rectTransform.rect;
				m_initSize.Set(rect.width, rect.height);
			}
		}

		private void RestoreInitValues()
		{
			RectTransform rectTransform = m_canvas.GetComponent<RectTransform>();
			rectTransform.position = m_initWorldspacePosition;
			rectTransform.rotation = m_initWorldspaceRotation;
			rectTransform.localScale = m_initWorldspaceScale;
			rectTransform.sizeDelta = m_initSize;
		}

		private void RegisterToEvents()
		{
			m_toggleUIRenderModeActionReference.action.performed += OnToggleUIRenderMode;
		}

		private void UnregisterFromEvents()
		{
			m_toggleUIRenderModeActionReference.action.performed -= OnToggleUIRenderMode;
		}

		#endregion
	}
}
