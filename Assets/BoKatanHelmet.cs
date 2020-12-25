using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BoKatanHelmet : MonoBehaviour
{
	public static BoKatanHelmet instance;
	VRTK_InteractableObject interactableObject;
	Rigidbody body;
	public AudioSource voSource;
	public GameObject legSnapper;
	public Collider col;
	private void Awake()
	{
		instance = this;
		interactableObject = GetComponent<VRTK_InteractableObject>();
		body = GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		interactableObject.InteractableObjectGrabbed += InteractableObject_InteractableObjectGrabbed;
		interactableObject.InteractableObjectUngrabbed += InteractableObject_InteractableObjectUngrabbed;
		interactableObject.InteractableObjectSnappedToDropZone += InteractableObject_InteractableObjectSnappedToDropZone;
	}

	public void InteractableObject_InteractableObjectSnappedToDropZone(object sender, InteractableObjectEventArgs e)
	{
		StartCoroutine(StopAudio());
		DisableGrabbing();
	}

	public void DisableGrabbing()
	{
		GetComponentInChildren<Outline>(true).enabled = false;
		GetComponent<Collider>().enabled = false;
	}

	private void OnDisable()
	{
		interactableObject.InteractableObjectGrabbed -= InteractableObject_InteractableObjectGrabbed;
		interactableObject.InteractableObjectUngrabbed -= InteractableObject_InteractableObjectUngrabbed;
		interactableObject.InteractableObjectSnappedToDropZone -= InteractableObject_InteractableObjectSnappedToDropZone;
	}


	private void InteractableObject_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
	{
		body.constraints = RigidbodyConstraints.None;
		if (isAllowing)
		{
			legSnapper.SetActive(false);
			col.enabled = true;
		}
	}
	private void InteractableObject_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
	{
		//body.constraints = RigidbodyConstraints.None;
	}


	IEnumerator StopAudio()
	{
		voSource.loop = false;
		while(voSource.isPlaying)
		{
			yield return null;
		}
		voSource.Stop();
	}

	bool isAllowing;
	[ContextMenu("AllowGrab")]
	public void AllowGrab()
	{
		GetComponent<Collider>().enabled = true;
		GetComponentInChildren<Outline>(true).enabled = true;
		isAllowing = true;
	}











}
