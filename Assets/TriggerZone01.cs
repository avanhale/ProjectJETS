using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone01 : MonoBehaviour
{
	public bool triggered;

	private void OnTriggerEnter(Collider other)
	{
		if (!triggered && other.GetComponentInParent<RazorCrest>())
		{
			FindObjectOfType<AsteroidShooter>().ShootAsteroids();
			triggered = true;
		}
	}
}
