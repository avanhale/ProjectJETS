using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
	public bool hasBeenTriggered;
	private void OnTriggerEnter(Collider other)
	{
		if (!hasBeenTriggered && other.GetComponentInParent<RazorCrest>())
		{
			FindObjectOfType<HyperSpaceButton>().ReadyHyperSpace();
			hasBeenTriggered = true;
		}
	}


}
