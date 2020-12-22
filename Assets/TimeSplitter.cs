using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeSplitter : MonoBehaviour
{
    public static TimeSplitter instance;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.G)) TimeSplit(0.5f, 1);
	}

	public void TimeSplit(float timeScale, float delay)
	{
		print("TImeSplit");
		StartCoroutine(TimeSplitRoutine(timeScale, delay));
	}

	IEnumerator TimeSplitRoutine(float timeScale, float delay)
	{
		yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScale, 1);
		yield return new WaitForSeconds(delay);
		yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 1);
	}


}
