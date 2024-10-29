using UnityEditor;
using UnityEngine;

namespace Plugins.EditorShortcuts.Editor
{
	public static class CopyPasteTransformComponent
	{
		#region Statics

		private static TransformData m_data;
		private static Vector3? m_dataCenter;

		#endregion

		#region PublicMethods

		[MenuItem("Edit/Copy Transform Values %&#c", false, -101)]
		public static void CopyTransformValues()
		{
			if (Selection.gameObjects.Length == 0)
			{
				return;
			}

			Transform selectionTr = Selection.gameObjects[0].transform;
			m_data = new TransformData(selectionTr.localPosition, selectionTr.localRotation, selectionTr.localScale);
		}

		[MenuItem("Edit/Paste Transform Values %&#v", false, -101)]
		public static void PasteTransformValues()
		{
			foreach (GameObject selection in Selection.gameObjects)
			{
				Transform selectionTr = selection.transform;
				Undo.RecordObject(selectionTr, "Paste Transform Values");
				selectionTr.localPosition = m_data.LocalPosition;
				selectionTr.localRotation = m_data.LocalRotation;
				selectionTr.localScale = m_data.LocalScale;
			}
		}

		[MenuItem("Edit/Copy Center Position %&#k", false, -101)]
		public static void CopyCenterPosition()
		{
			if (Selection.gameObjects.Length == 0)
			{
				return;
			}

			PivotMode currentPivotMode = Tools.pivotMode;
			Tools.pivotMode = PivotMode.Center;
			m_dataCenter = Tools.handlePosition;
			Tools.pivotMode = currentPivotMode;
		}

		[MenuItem("Edit/Paste Center Position %&#l", false, -101)]
		public static void PasteCenterPosition()
		{
			if (m_dataCenter == null)
			{
				return;
			}

			foreach (GameObject selection in Selection.gameObjects)
			{
				Undo.RecordObject(selection.transform, "Paste Center Position");
				selection.transform.position = m_dataCenter.Value;
			}
		}

		#endregion

		#region NestedTypes

		private struct TransformData
		{
			#region PublicFields

			public Vector3 LocalPosition;
			public Quaternion LocalRotation;
			public Vector3 LocalScale;

			#endregion

			#region Constructors

			public TransformData(Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
			{
				LocalPosition = localPosition;
				LocalRotation = localRotation;
				LocalScale = localScale;
			}

			#endregion
		}

		#endregion
	}
}
