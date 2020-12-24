using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
//using UnityEngine.PostProcessing;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	//public PostProcessingProfile profile;
	public bool useTimeScale;
	[Range(0, 10)]
	public float timeScale;

	public Color hitColor;

	public GameObject mover;

	private void Awake()
	{
		instance = this;
		//profile.grain.enabled = false;

		//VignetteModel.Settings vignette = profile.vignette.settings;
		//vignette.color = Color.black;
		//profile.vignette.settings = vignette;

	}

	private void Update()
	{
		if (useTimeScale)
		{
			Time.timeScale = timeScale;
		}
		else
			timeScale = Time.timeScale;

	}


	[ContextMenu("HyperSpace")]
	public void HyperSpaceHit()
	{
		print("HyperSpace!");
		VRTK_HeadsetFade.instance.Fade(Color.white, 5);
		AudioManager_JT.instance.HyperSpace();
		StartCoroutine(GrainShifter());
		VRTKCustom_Haptics.instance.HyperSpace();
	}

	IEnumerator GrainShifter()
	{
		//profile.grain.enabled = true;

		//float timer = 0;
		//while (timer < 5)
		//{
		//	timer += Time.deltaTime;
		//	float lerp = (float)(timer / 5f);
		//	GrainModel.Settings settings = profile.grain.settings;
		//	settings.intensity = 1 * lerp;
		//	profile.grain.settings = settings;

		//	yield return null;
		//}

		//profile.grain.enabled = false;
		yield return null;

	}



	[ContextMenu("HitIndication")]
	public void HitIndication()
	{
		StartCoroutine(HitRoutine());
		AudioManager_JT.instance.BlasterHit();
		VRTKCustom_Haptics.instance.Hit();
	}


	IEnumerator HitRoutine()
	{
		//VignetteModel.Settings vignette = profile.vignette.settings;
		float lerpTime = 0.5f;
		float curTime = 0;
		while (curTime < lerpTime)
		{
			curTime += Time.deltaTime;
			//vignette.color = Color.Lerp(Color.black, hitColor, curTime/lerpTime);
			//vignette.intensity = Mathf.Lerp(0.55f, 0.75f, curTime / lerpTime);
			//profile.vignette.settings = vignette;
			yield return null;
		}

		yield return new WaitForSeconds(0.25f);

		curTime = 0;
		lerpTime = 1;
		while (curTime < lerpTime)
		{
			curTime += Time.deltaTime;
			//vignette.color = Color.Lerp(hitColor, Color.black, curTime / lerpTime);
			//vignette.intensity = Mathf.Lerp(0.75f, 0.55f, curTime / lerpTime);
			//profile.vignette.settings = vignette;
			yield return null;
		}

	}






}
