using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RCLadder : MonoBehaviour
{
	public Transform teleportT;
	VRTK_InteractableObject interactableObject;

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
		StartCoroutine(UseLadder());
	}


	IEnumerator UseLadder()
	{
		VRTK_HeadsetFade.instance.Fade(Color.black, 1);
		yield return new WaitForSeconds(1);
		FindObjectOfType<BabyYoda>().Cooing();
		PlaySpaceRelativity.transformT.position = teleportT.position;
		PlaySpaceRelativity.transformT.rotation = teleportT.rotation;
		VRTK_HeadsetFade.instance.Unfade(1);

	}

}
