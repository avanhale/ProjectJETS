using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhistlingBirds : MonoBehaviour
{

	public static WhistlingBirds instance;
	public Transform cantinaT;
    WhistlingBird[] birds;
	bool hasFired;
	public GameObject colliderGO;
	private void Awake()
	{
		instance = this;
		birds = GetComponentsInChildren<WhistlingBird>();
		InitializeRockets();
		colliderGO.SetActive(false);

	}


	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("RightHand") && !hasFired)
		{
			ActivateRockets();
		}
	}

	public void CanShoot()
	{
		colliderGO.SetActive(true);
	}

	[ContextMenu("ShootRockets")]
	public void ActivateRockets()
	{
		StartCoroutine(ShootBirds());
		hasFired = true;
	}

	IEnumerator ShootBirds()
	{
		float delay = 0.5f;
		foreach (var bird in birds)
		{
			bird.Activate();
			yield return new WaitForSeconds(delay);
			delay *= 0.7f;
		}
	}

    void InitializeRockets()
	{
		Light[] lights = cantinaT.GetComponentsInChildren<Light>();
		for (int i = 0; i < birds.Length; i++)
		{
			birds[i].InitializeTarget(lights[i]);
		}
	}

}
