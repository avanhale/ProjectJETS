using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    public float cooldown;
	private void Start()
	{
		RotateAsteroids();
	}

	public void AsteroidHit(Vector3 hitPoint)
	{
        StartCoroutine(AsteroidHitRoutine(hitPoint));
	}

    IEnumerator AsteroidHitRoutine(Vector3 hitPoint)
	{
        AudioManager_JT.instance.AsteroidMetalHit(hitPoint);
        yield return new WaitForSeconds(cooldown);
    }


	void RotateAsteroids()
	{
		foreach (Transform child in transform)
		{
			child.rotation = Random.rotation;
			child.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * Random.Range(1000, 2000));
			child.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 10);
		}
	}

}
