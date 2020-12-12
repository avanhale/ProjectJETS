using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Controllables;
using DG.Tweening;

public class LiftThrusters : MonoBehaviour
{
    RazorCrest rc;
    int lastLift;
    AudioSource source;
    float startVol;
	private void Awake()
	{
        rc = GetComponentInParent<RazorCrest>();
        source = GetComponent<AudioSource>();
        startVol = source.volume;
    }
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rc.isDriving)
		{
            if (lastLift != rc.lift)
            {
                if (rc.lift == 0)
				{
                    StopAudio();
                }
                else
				{
                    PlayAudio();
                    if (rc.lift == 1)
					{
                        source.pitch = 1.2f;
					}
                    else if (rc.lift == -1)
					{
                        source.pitch = 0.8f;
					}
                }
                lastLift = rc.lift;
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
