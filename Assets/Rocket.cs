using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rocket : MonoBehaviour
{

    public Transform impT;
    public AudioSource source;
    public GameObject GO;

	private void Awake()
	{
        GO.SetActive(false);
    }

	[ContextMenu("Fire")]
    public void Fire()
	{
        StartCoroutine(FireRoutine());
	}

    IEnumerator FireRoutine()
    {
        transform.parent = null;// (null, true);
        yield return null;




        Sequence s = DOTween.Sequence();
        Vector3 targetPoint = impT.GetComponent<ImpDropShip>().NextPosition();
        //s.Append(transform.DOMove(targetPoint, 1.5f).SetEase(Ease.InOutSine).OnComplete(Completed));
        //transform.forward = targetPoint - transform.position;
        source.Play();
        GO.SetActive(true);

        float flyTime = 4f;
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
        impT.GetComponent<ImpDropShip>().Shot();
        GameManager.instance.EndGame();
	}



}
