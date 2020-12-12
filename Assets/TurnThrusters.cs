using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnThrusters : MonoBehaviour
{
    RazorCrest rc;
    AudioSource source;
    int lastTurn;
    float startVol;

    private void Awake()
	{
        rc = GetComponentInParent<RazorCrest>();
        source = GetComponent<AudioSource>();
        startVol = source.volume;
    }

    void Update()
    {
        if (rc.isDriving)
        {
            if (lastTurn != rc.turn)
            {
                if (rc.turn == 0)
                {
                    StopAudio();
                }
                else
                {
                    PlayAudio();
                }
                lastTurn = rc.turn;
            }
        }
    }


    void PlayAudio()
    {
        source.Play();
        source.DOFade(startVol, 0.25f);
    }

    void StopAudio()
    {
        source.DOFade(0, 0.25f);
    }
}
