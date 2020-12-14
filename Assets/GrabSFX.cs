using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class GrabSFX : MonoBehaviour
{
    public VRTK_InteractableObject gun;
	AudioSource source;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		gun.InteractableObjectGrabbed += Gun_InteractableObjectGrabbed;
	}
	private void OnDisable()
	{
		gun.InteractableObjectGrabbed -= Gun_InteractableObjectGrabbed;
	}
	private void Gun_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
	{
		source.Play();
		VRTKCustom_Haptics.instance.GrabBlaster();
	}

}
