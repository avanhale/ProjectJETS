using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidShooter : MonoBehaviour
{

	public Transform directorT;

	[ContextMenu("Shoot")]
    public void ShootAsteroids()
	{
		StartCoroutine(ShootRoutine());
	}

    IEnumerator ShootRoutine()
	{
		List<Rigidbody> bodies = new List<Rigidbody>();
		foreach (var asteroid in GetComponentsInChildren<Asteroid>())
		{
			bodies.Add(asteroid.GetComponent<Rigidbody>());
		}
		float delay = 0.75f;
		foreach (var body in bodies)
		{
			yield return new WaitForSeconds(delay);
			delay *= 0.98f;
			body.AddForce(directorT.forward * Random.Range(15000, 25000));
		}
	}


}
