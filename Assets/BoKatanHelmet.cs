using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BoKatanHelmet : MonoBehaviour
{
	VRTK_InteractableObject interactableObject;
	Rigidbody body;
	private void Awake()
	{
		interactableObject = GetComponent<VRTK_InteractableObject>();
		body = GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		interactableObject.InteractableObjectGrabbed += InteractableObject_InteractableObjectGrabbed;
		interactableObject.InteractableObjectUngrabbed += InteractableObject_InteractableObjectUngrabbed;
	}
	private void OnDisable()
	{
		interactableObject.InteractableObjectGrabbed -= InteractableObject_InteractableObjectGrabbed;
		interactableObject.InteractableObjectUngrabbed -= InteractableObject_InteractableObjectUngrabbed;
	}
	

	private void InteractableObject_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
	{
		body.constraints = RigidbodyConstraints.None;
	}
	private void InteractableObject_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
	{
		//body.constraints = RigidbodyConstraints.None;
	}
}
