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
    public AudioSource ost_Jawas;

    private void Awake()
	{
        instance = this;
	}

    public void AsteroidMetalHit(Vector3 pos)
	{
        asteroidMetalHit.transform.position = pos;
        asteroidMetalHit.PlayOneShot(asteroidMetalHit.clip);

    }

	private void Update()
	{
        //ost_Jawas.pitch = Mathf.Lerp(0, 1, Time.timeScale);
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

        GetComponent<AudioSource>().DOFade(0, 4).OnComplete(() => GetComponent<AudioSource>().Stop());

        timer = 0;
        while (timer < 5)
        {
            timer += Time.deltaTime;
            float lerp = (float)(timer / 5f);
            mixer.SetFloat("Volume", 0 - 50 * lerp);

            yield return null;
        }


        yield return new WaitForSeconds(1);

        timer = 0;
        while (timer < 5)
        {
            timer += Time.deltaTime;
            float lerp = (float)(timer / 5f);
            float vol = Mathf.Lerp(-50, 0, lerp);
            mixer.SetFloat("Volume", vol);

            yield return null;
        }



        mixer.SetFloat("Volume", 0);
        mixer.SetFloat("Pitch", 1);

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


    public void OST_Jawas()
	{
        ost_Jawas.Play();
	}


}
