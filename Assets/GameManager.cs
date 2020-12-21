using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.PostProcessing;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public PostProcessingProfile profile;
	public bool useTimeScale;
	[Range(0,10)]
	public float timeScale;
	private void Awake()
	{
		instance = this;
		profile.grain.enabled = false;
	}

	private void Update()
	{
		if (useTimeScale)
		{
			if (timeScale == 0) Time.timeScale = 1;
			else Time.timeScale = timeScale;
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
		profile.grain.enabled = true;

		float timer = 0;
		while (timer < 5)
		{
			timer += Time.deltaTime;
			float lerp = (float)(timer / 5f);
			GrainModel.Settings settings = profile.grain.settings;
			settings.intensity = 1 * lerp;
			profile.grain.settings = settings;

			yield return null;
		}

		profile.grain.enabled = false;

	}




	





}
