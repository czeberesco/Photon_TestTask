using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Plugins.EditorShortcuts.Editor
{
	public static class DuplicateWithoutAutoName
	{
		#region Statics

		private static List<int> m_selection = new();

		#endregion

		#region PublicMethods

		[MenuItem("Edit/Duplicate Without Auto-Name %&d", false, -101)]
		public static void DuplicateWithoutAutoNameExecute()
		{
			if (Selection.gameObjects.Length == 0)
			{
				return;
			}

			Undo.IncrementCurrentGroup();
			Undo.SetCurrentGroupName(nameof(DuplicateWithoutAutoNameExecute));

			foreach (GameObject selectedObject in Selection.gameObjects)
			{
				GameObject newGameObject = PrefabUtility.GetCorrespondingObjectFromSource(selectedObject);

				if (newGameObject != null)
				{
					newGameObject = PrefabUtility.InstantiatePrefab(newGameObject, selectedObject.transform.parent) as GameObject;
					PrefabUtility.SetPropertyModifications(newGameObject, PrefabUtility.GetPropertyModifications(selectedObject));
				}
				else
				{
					newGameObject = Object.Instantiate(selectedObject, selectedObject.transform.parent);
				}

				if (newGameObject != null)
				{
					Undo.RegisterCreatedObjectUndo(newGameObject, $"Duplicated game object {newGameObject.name}");

					newGameObject.transform.SetSiblingIndex(selectedObject.transform.GetSiblingIndex());
					newGameObject.name = selectedObject.name;
					m_selection.Add(newGameObject.GetInstanceID());
				}
			}

			Selection.instanceIDs = m_selection.ToArray();
			m_selection.Clear();

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
		}

		#endregion
	}
}
