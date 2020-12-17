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
	private void Awake()
	{
        anim = GetComponentInChildren<Animator>();
        Invoke("StopAnim", .75f);
        anim.transform.localPosition = anim.transform.localPosition.WithY(-3);
        holeT.localPosition = holeT.localPosition.WithY(-1);
    }

    [ContextMenu("Reveal")]
    public void Reveal()
	{
        anim.transform.DOLocalMoveY(1.5f, 1.5f).SetEase(Ease.InOutCubic);
        DOTween.To(() => anim.speed, x => anim.speed = x, 1, 2f);
        holeT.DOLocalMoveY(0, 1f);
        burrowSource.Play();
        screechSource.Play();
        Invoke("LowPitch", 3.5f);
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
        screechSource.pitch = 0.8f;
        screechSource.volume = 0.85f;
	}



    void StopAnim()
	{
        anim.speed = 0;
        Reveal();
	}


    public void Damage(int damage)
	{
        Burrow();
	}


}
