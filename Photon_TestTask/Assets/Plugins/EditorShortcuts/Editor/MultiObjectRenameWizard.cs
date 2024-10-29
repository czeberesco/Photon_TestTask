using UnityEditor;
using UnityEngine;

namespace Plugins.EditorShortcuts.Editor
{
	public class MultiObjectRenameWizard : ScriptableWizard
	{
		#region PublicFields

		public string OldToken;
		public string NewToken;

		#endregion

		#region PublicMethods

		[MenuItem("Edit/Multi-Object Rename Wizard %&#f", false, -101)]
		public static void CreateWizard()
		{
			ScriptableWizard sw = DisplayWizard(
				"Multi-Object Rename Wizard",
				typeof(MultiObjectRenameWizard),
				"Rename"
			);
		}

		#endregion

		#region PrivateMethods

		private void OnWizardCreate()
		{
			foreach (GameObject selectedObject in Selection.gameObjects)
			{
				string objName = selectedObject.name;

				if (objName.Contains(OldToken))
				{
					Undo.RecordObject(selectedObject, "Multi-Object Rename Wizard");
					selectedObject.name = objName.Replace(OldToken, NewToken);
				}
			}
		}

		private void OnWizardUpdate()
		{
			isValid = !string.IsNullOrEmpty(OldToken);
		}

		#endregion
	}
}
