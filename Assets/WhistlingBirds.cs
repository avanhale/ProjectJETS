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
	public GameObject smokePX;

	public MeshRenderer[] lights;
	private void Awake()
	{
		instance = this;
		birds = GetComponentsInChildren<WhistlingBird>();
		InitializeRockets();
		//colliderGO.SetActive(false);

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
		StartCoroutine(BirdsRoutine());
		hasFired = true;
	}

	IEnumerator BirdsRoutine()
	{
		smokePX.SetActive(true);
		yield return ShootBirds();
		yield return new WaitForSeconds(0.5f);
		AmbientLighter.instance.Cantina();
		foreach (var mesh in lights)
		{
			Material mat = mesh.material;
			mat.DisableKeyword("_EMISSION");
			mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
			mat.SetColor("_EmissionColor", Color.black);
		}
		colliderGO.SetActive(false);

	}

	IEnumerator ShootBirds()
	{
		float delay = 0.5f;
		foreach (var bird in birds)
		{
			VRTKCustom_Haptics.instance.BirdBuzz();
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
