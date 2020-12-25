using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

public class ImpDropShip : MonoBehaviour
{
    BezierWalkerWithSpeed walker;
    GameObject shipGO;
    public GameObject pxGo;

	private void Awake()
	{
        walker = GetComponent<BezierWalkerWithSpeed>();
        walker.enabled = false;
        shipGO = transform.GetChild(0).gameObject;
        shipGO.SetActive(false);
        pxGo.SetActive(false);
    }


    public void EnableShip()
	{
        shipGO.SetActive(true);
    }


    public void StartFly()
	{
        EnableShip();
        walker.enabled = true;
	}


    public void Shot()
	{
        StartCoroutine(ShotRoutine());
    }

    IEnumerator ShotRoutine()
	{
        GetComponent<AudioSource>().Play();
        pxGo.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shipGO.SetActive(false);

    }

    public Vector3 NextPosition()
	{
        return walker.spline.GetPoint(walker.NormalizedT + 0.1f);
	}



}
