using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPoint : MonoBehaviour
{
    public bool isTaken;


    public void TakePoint()
	{
		isTaken = true;
	}


	public void ReleasePoint()
	{
		isTaken = false;
	}

	public Vector3 Position()
	{
		return transform.position;
	}


}
