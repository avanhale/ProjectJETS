using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LandedSFX : MonoBehaviour
{
	public VRTK_BodyPhysics body;
	AudioSource source;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		body.StopFalling += Body_StopFalling;
	}
	private void OnDisable()
	{
		body.StopFalling -= Body_StopFalling;
	}
	private void Body_StopFalling(object sender, BodyPhysicsEventArgs e)
	{
		source.PlayOneShot(source.clip);
	}
}
