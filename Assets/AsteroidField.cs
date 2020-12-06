using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    public float cooldown;

	public void AsteroidHit(Vector3 hitPoint)
	{
        StartCoroutine(AsteroidHitRoutine(hitPoint));
	}

    IEnumerator AsteroidHitRoutine(Vector3 hitPoint)
	{
        AudioManager_JT.instance.AsteroidMetalHit(hitPoint);
        yield return new WaitForSeconds(cooldown);
    }


}
