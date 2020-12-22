using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sandworm : MonoBehaviour
{
    Animator anim;
    public Transform holeT;
    public AudioSource burrowSource, screechSource;
    bool isBurrowing;
    public bool isBIG;
    public bool useSlam;
    int numHits;
    int targetHits = 2;
    public Transform baseT;
    public float slamDelay;

    public AudioSource slamSource;
    public GameObject smokePXGO, explosionPXGO;

    private void Awake()
	{
        anim = GetComponentInChildren<Animator>();
        Invoke("StopAnim", .75f);
        anim.transform.localPosition = anim.transform.localPosition.WithY(-3);
        holeT.localPosition = holeT.localPosition.WithY(-1);
        if (isBIG)
        {
            smokePXGO.SetActive(false);
            if (!useSlam) explosionPXGO.SetActive(false);
        }
    }

    [ContextMenu("Reveal")]
    public void Reveal()
    {
        anim.transform.DOLocalMoveY(1.5f, isBIG ? 3.5f : 1.5f).SetEase(Ease.InOutCubic);
        DOTween.To(() => anim.speed, x => anim.speed = x, isBIG ? 0.6f : 1, 2f);
        holeT.DOLocalMoveY(0, isBIG ? 2.75f : 1f);
        burrowSource.Play();
        screechSource.Play();
        Invoke("LowPitch", isBIG ? 5.75f : 4f);

        if (isBIG && useSlam)
        {
            Invoke("Slam", slamDelay);
            smokePXGO.SetActive(true);
        }
    }

    [ContextMenu("Slam")]
    public void Slam()
	{
        DOTween.To(() => anim.speed, x => anim.speed = x, 0, 4f);
        baseT.DOLocalRotate(Vector3.left * 75, 4.8f).SetEase(Ease.InQuad).OnComplete(() => VRTKCustom_Haptics.instance.WormSlam());
        slamSource.gameObject.SetActive(true);
        Invoke("Destroy", 37);
    }


    [ContextMenu("Burrow")]
    public void Burrow()
	{
        if (isBurrowing) return;

        anim.transform.DOLocalMoveY(-3f, 1.5f).SetEase(Ease.InOutCubic);
        DOTween.To(() => anim.speed, x => anim.speed = x, 0, 2f);
        holeT.DOLocalMoveY(-1, 8f);

        burrowSource.Play();
        screechSource.loop = false;
        isBurrowing = true;
    }

    void LowPitch()
	{
        screechSource.pitch /= 1.4f;
        screechSource.volume *= 0.85f;
	}


    IEnumerator HitAnim()
	{
        float speed = anim.speed;
        anim.speed *= 2;
        yield return new WaitForSeconds(1);
        anim.speed  = speed;

    }


    void StopAnim()
	{
        anim.speed = 0;
        //Reveal();
	}


    public void Damage(int damage)
	{
        if (!isBurrowing)
        {
            numHits++;
            StartCoroutine(HitAnim());
            if (numHits == targetHits)
            {
                Burrow();
            }

        }
	}



    void Destroy()
	{
        baseT.gameObject.SetActive(false);
	}


}
