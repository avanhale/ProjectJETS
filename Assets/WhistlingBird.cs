using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WhistlingBird : MonoBehaviour
{
    Light targetLight;
    AudioSource source;
    public AudioSource lightSource;
    TrailRenderer trail;

	private void Awake()
	{
        source = GetComponent<AudioSource>();
        trail = GetComponentInChildren<TrailRenderer>();
        trail.enabled = false;
    }

    [ContextMenu("Activate")]
    public void Activate()
	{
        StartCoroutine(Fire());
	}

    IEnumerator Fire()
	{
        transform.parent = null;// (null, true);
        yield return null;
        Sequence s = DOTween.Sequence();
        Vector3 targetPoint = targetLight.transform.position;
        s.Append(transform.DOMove(targetPoint, 1.5f).OnComplete(Completed));
        s.Join(transform.DORotateQuaternion(Quaternion.LookRotation(targetPoint - transform.position, Vector3.up), .25f));
        source.Play();
        trail.enabled = true;
	}

    void Completed()
	{
        targetLight.enabled = false;
        trail.enabled = false;
        lightSource.Play();
        Invoke("gone", 2);
    }

    void gone()
	{
        gameObject.SetActive(false);
	}


    public void InitializeTarget(Light light)
	{
        targetLight = light;
    }


}
