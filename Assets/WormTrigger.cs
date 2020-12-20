using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTrigger : MonoBehaviour
{
	public Sandworm sandworm;
	bool triggered;
	private void OnTriggerEnter(Collider other)
	{
		OVRCameraRig player = other.GetComponentInParent<OVRCameraRig>();
		if (player && !triggered)
		{
			triggered = true;
			sandworm.Reveal();
		}
	}
}
