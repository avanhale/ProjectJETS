using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HyperSpaceButton : MonoBehaviour
{
    public bool isHyperSpaceReady;
    Collider collider;
	Outline outline;
	MeshRenderer mesh;

	private void Awake()
	{
		collider = GetComponent<Collider>();
		collider.enabled = false;
		outline = GetComponent<Outline>();
		outline.enabled = false;
		mesh = GetComponent<MeshRenderer>();
	}

	[ContextMenu("Ready")]
	public void ReadyHyperSpace()
	{
		collider.enabled = true;
		outline.enabled = true;
		isHyperSpaceReady = true;
		mesh.material.DOColor(mesh.material.color * 0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
		{
			if (isHyperSpaceReady)
			{
				GameManager.instance.HyperSpaceHit();
				GetComponent<AudioSource>().Play();
				isHyperSpaceReady = false;
			}
		}
	}



}
