using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.PostProcessing;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public PostProcessingProfile profile;
	private void Awake()
	{
		instance = this;
		profile.grain.enabled = false;
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
