using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodEventManager : MonoBehaviour
{
    public static PodEventManager instance;

	public PodRacer podRacer;
	public TuskenBiker[] bikers;
	public float[] delays;

	public Sandworm sandwormBig;
	bool wormRevealed;
	public float wormTrigger;

	bool dark1, dark2;
	public float darkTrigger1, darkTrigger2;

	bool light1;
	public float lightTrigger1;

	private void Awake()
	{
        instance = this;
	}

	private void Update()
	{
		if (!wormRevealed && podRacer.m_NormalizedT > wormTrigger)
		{
			sandwormBig.Reveal();
			wormRevealed = true;
		}

		if (!dark1 && podRacer.m_NormalizedT > darkTrigger1)
		{
			AmbientLighter.instance.Dark();
			dark1 = true;
		}
		if (!dark2 && podRacer.m_NormalizedT > darkTrigger2)
		{
			AmbientLighter.instance.Dark();
			dark2 = true;
		}

		if (!light1 && podRacer.m_NormalizedT > lightTrigger1)
		{
			AmbientLighter.instance.Normal();
			light1 = true;
		}
	}

	public void StartRacing()
	{
		StartCoroutine(RaceRoutine());
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





}
