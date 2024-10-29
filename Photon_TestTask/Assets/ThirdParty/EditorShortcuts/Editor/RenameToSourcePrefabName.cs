using UnityEditor;
using UnityEngine;

namespace Plugins.EditorShortcuts.Editor
{
	public static class RenameToSourcePrefabName
	{
		#region PublicMethods

		[MenuItem("Edit/Rename to source prefab name %&#t", false, -101)]
		public static void ExecuteRenameToPrefabName()
		{
			RemoveFromObjects(Selection.gameObjects);
		}

		#endregion

		#region PrivateMethods

		private static void RemoveFromObjects(params GameObject[] objects)
		{
			foreach (GameObject selectedObject in objects)
			{
				if (selectedObject.scene.IsValid() &&
					PrefabUtility.GetPrefabInstanceStatus(selectedObject) == PrefabInstanceStatus.Connected &&
					PrefabUtility.IsAnyPrefabInstanceRoot(selectedObject))
				{
					Undo.RecordObject(selectedObject, "Rename to source prefab name");

					GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(selectedObject);
					selectedObject.name = sourcePrefab.name;
				}
			}
		}

		#endregion
	}
}
