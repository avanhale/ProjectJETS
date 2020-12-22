using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager_JT : MonoBehaviour
{
    public static AudioManager_JT instance;
    public AudioMixer mixer;
    public AudioSource asteroidMetalHit;
    public AudioSource blasterHit;
    public AudioSource forceImpact;

    private void Awake()
	{
        instance = this;
	}

    public void AsteroidMetalHit(Vector3 pos)
	{
        asteroidMetalHit.transform.position = pos;
        asteroidMetalHit.PlayOneShot(asteroidMetalHit.clip);

    }



    [ContextMenu("Hyper")]
    public void HyperSpace()
	{
        StartCoroutine(PitchShifter());
    }

    IEnumerator PitchShifter()
	{
        float timer = 0;
        while (timer < 5)
		{
            timer += Time.deltaTime;
            float lerp = (float)(timer / 5f);
            mixer.SetFloat("Pitch", 1 + 2 * lerp);

            yield return null;
		}

        timer = 0;
        while (timer < 5)
        {
            timer += Time.deltaTime;
            float lerp = (float)(timer / 5f);
            mixer.SetFloat("Volume", 0 - 50 * lerp);

            yield return null;
        }

        yield return new WaitForSeconds(5);
        mixer.SetFloat("Volume", 0);

    }




    public void BlasterHit()
	{
        blasterHit.Play();
	}


    public void ForceImpact(Vector3 pos)
	{
        forceImpact.transform.position = pos;
        forceImpact.Play();
	}





}
