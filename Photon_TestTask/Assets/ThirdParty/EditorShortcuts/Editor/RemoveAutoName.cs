using System;
using UnityEditor;
using UnityEngine;

namespace Plugins.EditorShortcuts.Editor
{
	public static class RemoveAutoName
	{
		#region PublicMethods

		[MenuItem("Edit/Remove Auto-Name %&#r", false, -101)]
		public static void RemoveFromSelection()
		{
			RemoveFromObjects(Selection.gameObjects);
		}

		#endregion

		#region PrivateMethods

		private static void RemoveFromObjects(params GameObject[] objects)
		{
			foreach (GameObject selectedObject in objects)
			{
				string objName = selectedObject.name;
				char lastChar = objName[^1];
				int indexOfAddedString = objName.LastIndexOf(" (", StringComparison.Ordinal);
				int uniqueNumberLength = objName.Length - (indexOfAddedString + 3);
				string uniqueNumberString = objName.Substring(indexOfAddedString + 2, uniqueNumberLength);

				if (lastChar == ')' && indexOfAddedString != -1 && int.TryParse(uniqueNumberString, out int _))
				{
					Undo.RecordObject(selectedObject, "Remove Auto-Name");
					selectedObject.name = objName[..indexOfAddedString];
				}
			}
		}

		#endregion
	}
}
