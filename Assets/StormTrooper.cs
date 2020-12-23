using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StormTrooper : MonoBehaviour
{
    AudioSource source;
	NavMeshAgent agent;
	public Transform targetT;
	Animator anim;

	private void Awake()
	{
        source = GetComponentInChildren<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();
	}
	void OnAnimatorMove()
	{
		// Update position based on animation movement using navigation surface height
		Vector3 position = anim.rootPosition;
		position.y = agent.nextPosition.y;
		transform.position = position;
	}

	private void Update()
	{
		agent.destination = targetT.position;
		//agent.Move(targetT.position - transform.position);

		float forward = agent.velocity.magnitude;
		anim.SetFloat("Forward", forward);

	}

	public void Damage(int damage)
	{
        source.Play();
    }


}
