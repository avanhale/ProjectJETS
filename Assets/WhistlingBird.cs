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
    public GameObject meshes;

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
        //yield return transform.DOMove(transform.position + transform.forward * 0.25f, 0.1f).SetEase(Ease.InSine).WaitForCompletion();

        Sequence s = DOTween.Sequence();
        Vector3 targetPoint = targetLight.transform.position;
        //s.Append(transform.DOMove(targetPoint, 1.5f).SetEase(Ease.InOutSine).OnComplete(Completed));
        //transform.forward = targetPoint - transform.position;
        source.Play();
        trail.enabled = true;

        float flyTime = 1.75f;
        float curTime = 0;
        while (curTime < flyTime)
		{
            yield return null;
            curTime += Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 180 * Time.deltaTime);
            float speed = Mathf.Lerp(5, 20, curTime / flyTime);
            transform.position += transform.forward * speed * Time.deltaTime;
		}
        Completed();
        //s.Join(transform.DORotateQuaternion(Quaternion.LookRotation(targetPoint - transform.position, Vector3.up), .25f));
	}

    void Completed()
	{
        targetLight.enabled = false;
        trail.enabled = false;
        lightSource.PlayOneShot(lightSource.clip);
        meshes.SetActive(false);
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
