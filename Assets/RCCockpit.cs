using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RCCockpit : MonoBehaviour
{
	VRTK_InteractableObject interactableObject;
	bool triggered;

	private void Awake()
	{
		interactableObject = GetComponent<VRTK_InteractableObject>();
	}

	private void OnEnable()
	{
		interactableObject.InteractableObjectUsed += InteractableObject_InteractableObjectUsed;
	}
	private void OnDisable()
	{
		interactableObject.InteractableObjectUsed -= InteractableObject_InteractableObjectUsed;
	}

	private void InteractableObject_InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
	{
		if (!triggered)
		{
			RazorCrest.instance.EnterDrivingSeat();
			triggered = true;
			gameObject.SetActive(false);
		}
	}

}
