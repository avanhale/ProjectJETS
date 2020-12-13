using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LerpSky : MonoBehaviour
{
    public Material skyMat;
	public float ogExposure;

	private void Start()
	{
	}

	private void Update()
	{
		skyMat.SetFloat("_Exposure", ogExposure * (Mathf.Sin(Time.time) * 0.05f + 1f));
	}

}
