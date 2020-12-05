using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTrooper : MonoBehaviour
{
    AudioSource source;

	private void Awake()
	{
        source = GetComponentInChildren<AudioSource>();
	}



	public void Damage(int damage)
	{
        source.Play();
    }


}
