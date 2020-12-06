using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_JT : MonoBehaviour
{
    public static AudioManager_JT instance;
    public AudioSource asteroidMetalHit;

	private void Awake()
	{
        instance = this;
	}

    public void AsteroidMetalHit(Vector3 pos)
	{
        asteroidMetalHit.transform.position = pos;
        asteroidMetalHit.PlayOneShot(asteroidMetalHit.clip);

    }
}
