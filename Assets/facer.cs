using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class facer : MonoBehaviour
{

	private void Start()
	{
		gameObject.SetActive(false);
	}

	[ContextMenu("Activate")]
	public void Activate()
	{
		gameObject.SetActive(true);
		GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
		GetComponentInChildren<TextMeshProUGUI>().DOColor(Color.white, 10);
	}

	private void Update()
	{
		transform.forward = transform.position - PlaySpaceRelativity.cameraT.position;
	}


}
