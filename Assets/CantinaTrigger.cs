using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantinaTrigger : MonoBehaviour
{

	bool triggered;

	private void OnTriggerEnter(Collider other)
	{
		OVRCameraRig rig = other.GetComponentInParent<OVRCameraRig>();
		if (!triggered && rig)
		{
			GameManager.instance.CantinaEvent();
			triggered = true;
		}
	}









}
