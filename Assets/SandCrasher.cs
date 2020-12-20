using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandCrasher : MonoBehaviour
{
    bool triggered;




	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PodRacer") && !triggered)
		{
			GetComponent<AudioSource>().Play();
			triggered = true;
		}
	}


}
