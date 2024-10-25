using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VR.Interactions.Interactors
{
	public class XRSimpleGrabInteractable : XRBaseInteractable
	{
		#region ProtectedMethods

		protected override void OnSelectEntered(SelectEnterEventArgs args)
		{
			Debug.Log("Select entered");
		}

		protected override void OnSelectExited(SelectExitEventArgs args)
		{
			Debug.Log("Select exited");
		}

		#endregion
	}
}
