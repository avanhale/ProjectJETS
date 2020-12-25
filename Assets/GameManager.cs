using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public PostProcessProfile profile;
	public PostProcessProfile targetProfile;
	public bool useTimeScale;
	[Range(0, 10)]
	public float timeScale;

	public Material tatooineSky, spaceSky;

	public GameObject tatooineGO, moonGO;
	public Transform JTLanding, RCLanding;

	public Color hitColor;

	public GameObject mover;

	public ig11 eyegee;

	public StormTrooper cantinaTrooper;
	public Transform stormTroopersT;
	public ImpDropShip dropShip;
	public GameObject targeter;
	public facer face;

	public GameObject bodyCol;

	private void Awake()
	{
		instance = this;
		targeter.SetActive(false);
	}

	private void Start()
	{
		StartCoroutine(StartGame());
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

	IEnumerator StartGame()
	{

		yield return null;
		VRTK_HeadsetFade.instance.Fade(Color.black, 0);
		yield return new WaitForSeconds(5);
		tatooineGO.SetActive(false);

		VRTK_HeadsetFade.instance.Unfade(1);
		// RC delay
		yield return new WaitForSeconds(75);
		RazorCrest.instance.Starting();

	}


	[ContextMenu("HyperSpace")]
	public void HyperSpaceHit()
	{
		print("HyperSpace!");
		StartCoroutine(PlanetShifter());
	}

	IEnumerator PlanetShifter()
	{
		VRTK_HeadsetFade.instance.Fade(Color.white, 5);
		AudioManager_JT.instance.HyperSpace();
		VRTKCustom_Haptics.instance.HyperSpace();
		yield return new WaitForSeconds(5);
		RenderSettings.fog = true;
		RenderSettings.skybox = tatooineSky;
		moonGO.SetActive(false);
		tatooineGO.SetActive(true);
		GetComponent<PostProcessVolume>().profile = profile;
		yield return new WaitForSeconds(7);

		RazorCrest.instance.ExitDrivingSeat();
		RazorCrest.instance.Landing();
		yield return new WaitForSeconds(2);
		Transform t = RazorCrest.instance.transform;
		t.position = RCLanding.position;
		t.rotation = RCLanding.rotation;

		Transform p = PlaySpaceRelativity.transformT;
		p.position = JTLanding.position;
		p.rotation = JTLanding.rotation;
		VRTK_HeadsetFade.instance.Unfade(5);




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


	public void CantinaEvent()
	{
		StartCoroutine(CantinaRoutine());
	}

	IEnumerator CantinaRoutine()
	{
		BoKatanHelmet.instance.AllowGrab();
		yield return new WaitForSeconds(2);
		eyegee.CantinaEvent();

	}

	public void CantinaEvent2()
	{
		StartCoroutine(CantinaRoutine2());
	}

	IEnumerator CantinaRoutine2()
	{
		yield return new WaitForSeconds(2.75f);
		eyegee.DangerMode();
		yield return new WaitForSeconds(2.75f);
		cantinaTrooper.StartCantinaRoutine();

		yield return new WaitForSeconds(10f);
		WhistlingBirds.instance.CanShoot();
	}

	public void Bloom(bool bloom)
	{
		Bloom b;
		profile.TryGetSettings<Bloom>(out b);
		b.enabled.Override(bloom);
	}




	[ContextMenu("FireScene")]
	public void StartFireScene()
	{
		foreach (var trooper in stormTroopersT.GetComponentsInChildren<StormTrooper>())
		{
			trooper.StartFireScene();
		}

		dropShip.EnableShip();
		StartCoroutine(TroopChecker());
		bodyCol.SetActive(true);
	}

	IEnumerator TroopChecker()
	{
		yield return new WaitForSeconds(5);

		while (!AllDead())
		{
			yield return new WaitForSeconds(2);
		}

		bool AllDead()
		{
			foreach (var trooper in stormTroopersT.GetComponentsInChildren<StormTrooper>())
			{
				if (!trooper.isDead) return false;
			}
			return true;
		}
		EndFireScene();
	}


	public void EndFireScene()
	{
		Invoke("StartImpShip", 5);
	}



	[ContextMenu("IMpShip")]
	public void StartImpShip()
	{
		dropShip.StartFly();
		targeter.SetActive(true);
		GetComponent<PostProcessVolume>().profile = targetProfile;
	}


	public void EndGame()
	{
		StartCoroutine(EndGamer());
	}

	IEnumerator EndGamer()
	{
		targeter.gameObject.SetActive(false);
		face.Activate();
		yield return new WaitForSeconds(7);
		AudioSource music = GetComponent<AudioSource>();
		music.volume = 0;
		music.DOFade(1, 10);
		music.Play();
		VRTK_HeadsetFade.instance.Fade(Color.black, 5);

	}

	public void SetProfile()
	{
		GetComponent<PostProcessVolume>().profile = profile;
	}



}
