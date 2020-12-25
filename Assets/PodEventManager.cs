using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VRTK;

public class PodEventManager : MonoBehaviour
{
    public static PodEventManager instance;

	public PodRacer podRacer;
	public TuskenBiker[] bikers;
	public float[] delays;

	public Sandworm sandwormBig;
	public Sandworm sandwormBig2;
	bool wormRevealed;
	public float wormTrigger;

	bool wormRevealed2;
	public float wormTrigger2;

	public GameObject cavesReverb;

	bool dark1;
	public float darkTrigger1;

	bool light1;
	public float lightTrigger1;

	bool bit;
	public float bitTrigger;

	bool reverb;
	public float reverbTrigger;

	bool rockTriggered;
	public float rockTrigger;
	public Transform rockT;

	bool wormjump;
	public float jumpTrigger;

	bool jetpacked;

	public AudioSource explosionSFX;

	private void Awake()
	{
        instance = this;
	}

	private void OnEnable()
	{
		JetPack.OnJetPackPressed += JetPack_OnJetPackPressed;
	}
	private void OnDisable()
	{
		JetPack.OnJetPackPressed -= JetPack_OnJetPackPressed;
	}

	private void JetPack_OnJetPackPressed()
	{
		if (wormjump && !jetpacked)
		{
			StartCoroutine(JumpOutOfWorm());
			jetpacked = true;
		}
	}

	private void Update()
	{
		if (!wormRevealed && podRacer.m_NormalizedT > wormTrigger)
		{
			sandwormBig.Reveal();
			Invoke("IncreaseSpeed", 9.25f);
			wormRevealed = true;
		}

		if (!wormRevealed2 && podRacer.m_NormalizedT > wormTrigger2)
		{
			sandwormBig2.Reveal();
			wormRevealed2 = true;
		}

		if (!dark1 && podRacer.m_NormalizedT > darkTrigger1)
		{
			AmbientLighter.instance.Dark();
			dark1 = true;
			cavesReverb.SetActive(true);
			GameManager.instance.Bloom(false);
		}

		if (!light1 && podRacer.m_NormalizedT > lightTrigger1)
		{
			AmbientLighter.instance.Light();
			light1 = true;
			GameManager.instance.Bloom(true);
		}


		if (!reverb && podRacer.m_NormalizedT > reverbTrigger)
		{
			cavesReverb.SetActive(false);
			reverb = true;
		}


		if (!rockTriggered && podRacer.m_NormalizedT > rockTrigger)
		{
			LiftRock();
			rockTriggered = true;
		}

		if (!bit && podRacer.m_NormalizedT > bitTrigger)
		{
			GameManager.instance.HitIndication();
			bit = true;
		}

		if (!wormjump && podRacer.m_NormalizedT > jumpTrigger)
		{
			BigWormJump();
			wormjump = true;
		}

	}

	public void StartRacing()
	{
		StartCoroutine(RaceRoutine());
		FindObjectOfType<Sandcrawler>().StartTrack();
	}

	IEnumerator RaceRoutine()
	{
		int bikerIndex = 0;
		while (bikerIndex < bikers.Length)
		{
			StartCoroutine(BikerStarter(bikerIndex));
			bikerIndex++;
		}

		yield return null;

	}

	IEnumerator BikerStarter(int index)
	{
		yield return new WaitForSeconds(delays[index]);
		bikers[index].StartMoving();
	}


	void IncreaseSpeed()
	{
		podRacer.baseSpeed = 25;
	}



	[ContextMenu("LiftRock")]
	public void LiftRock()
	{
		StartCoroutine(LiftRoutine());
	}

	IEnumerator LiftRoutine()
	{
		BabyYoda.instance.Efforts();
		float liftTime = 5.5f;
		Tween shaker = rockT.DOShakePosition(liftTime, 1.25f, 12);
		yield return new WaitForSeconds(0.5f);
		Tween t = rockT.DOLocalMoveY(25, liftTime).SetEase(Ease.OutSine).SetEase(Ease.InSine);
		rockT.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(1);
		AudioManager_JT.instance.ForceImpact(PodRacer.instance.racerLine.GetPoint(PodRacer.instance.m_NormalizedT + 0.005f));
	}



	[ContextMenu("BigWormJump")]
	public void BigWormJump()
	{
		timeScaleTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, 5).SetEase(Ease.OutSine);
		JetPack.instance.isJetting = false;
		JetPack.instance.canJets = false;
	}

	Tween timeScaleTween;
	IEnumerator JumpOutOfWorm()
	{
		print("Worm Jumped!");

		timeScaleTween.Kill();
		DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 0.125f).SetEase(Ease.InQuad);

		FindObjectOfType<OVRCameraRig>().transform.SetParent(null);
		VRTK.VRTK_BodyPhysics bodyPhysics = FindObjectOfType<VRTK.VRTK_BodyPhysics>();
		bodyPhysics.transform.SetParent(null);
		bodyPhysics.enableBodyCollisions = true;
		GameManager.instance.mover.SetActive(true);


		podRacer.DropShip();
		StartCoroutine(Explosion());

		yield return new WaitForSeconds(0.025f);
		JetPack.instance.EndJets();
		bodyPhysics.ApplyBodyVelocity(Vector3.up * 100f + Vector3.back * 50f + Vector3.left * 35f, true, true);
		//JetPack.instance.Jets();

		yield return new WaitUntil(() => bodyPhysics.GetVelocity().y < 5);
		//JetPack.instance.StartJets();
		JetPack.instance.canJets = true;

		FindObjectOfType<Sandcrawler>().StopTrack();


	}

	IEnumerator Explosion()
	{
		explosionSFX.Play();
		yield return new WaitForSeconds(0.4f);
		sandwormBig2.explosionPXGO.SetActive(true);
		VRTKCustom_Haptics.instance.WormSlam();
		yield return new WaitForSeconds(0.65f);
		sandwormBig2.Burrow();
		podRacer.gameObject.SetActive(false);

	}






}
