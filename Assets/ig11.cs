using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ig11 : MonoBehaviour
{
    public Transform midT, eyesT;
	public float targetMid, midSpeed;
	public float midLerp;

	public float targetEye, eyeSpeed;
	public float eyeLerp;
	Transform playerCamT;
	public Transform shoulderT;


	public AudioSource sfx01, sfx02;
	public GameObject gun;

	private void Start()
	{
		StartCoroutine(Midder());
		StartCoroutine(Eyer());
		playerCamT = PlaySpaceRelativity.cameraT;
		gun.SetActive(false);
	}

	private void LateUpdate()
	{
		targetMid += midSpeed * Time.deltaTime;
		midT.localEulerAngles = Vector3.Lerp(midT.localEulerAngles, midT.localEulerAngles.WithY(targetMid), midLerp);
		eyesT.localEulerAngles = Vector3.Lerp(eyesT.localEulerAngles, eyesT.localEulerAngles.WithY(targetEye), eyeLerp);


		if (isDanger)
		{
			//shoulderT.rotation = Quaternion.LookRotation(Vector3.down, )
			shoulderT.up = playerCamT.position - shoulderT.position;
			shoulderT.Rotate(Vector3.down * 90, Space.Self);
		}


	}


	IEnumerator Midder()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(5, 15));
			RandomMid();
		}
	}

	void RandomMid()
	{
		targetMid = Random.Range(0, 360);
		midSpeed = Random.Range(-10, 10);
	}

	IEnumerator Eyer()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(5, 15));
			RandomEye();
		}
	}

	void RandomEye()
	{
		targetEye = Random.Range(0, 360);
		eyeSpeed = Random.Range(-10, 10);
	}

	[ContextMenu("CantinaEvent")]
	public void CantinaEvent()
	{
		sfx01.Play();
	}

	//IEnumerator EventRoutine()
	//{
	//	sfx01.Play();
	//	yield return new WaitForSeconds(sfx01.clip.length);
	//	yield return new WaitForSeconds(2);
	//	DangerMode();
	//}



	public bool isDanger;
	[ContextMenu("DangerMode")]
	public void DangerMode()
	{
		isDanger = true;
		sfx02.Play();
		Invoke("RepeatSFX", 6);
		gun.SetActive(true);
	}

	void RepeatSFX()
	{
		sfx01.loop = true;
		sfx01.pitch *= 0.95f;
		sfx01.Play();
	}



}
