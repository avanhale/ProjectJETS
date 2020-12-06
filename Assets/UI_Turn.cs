using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Turn : MonoBehaviour
{
    public Transform barT;
    RazorCrest razorCrest;
    public float targetVal;

	private void Awake()
	{
        razorCrest = GetComponentInParent<RazorCrest>();
    }

    void Update()
    {
        UpdateTurn();
        barT.localPosition = Vector3.Lerp(barT.localPosition, Vector3.forward * 0.075f * targetVal, 0.025f);
    }


    void UpdateTurn()
	{
        int turn = razorCrest.turn;
        targetVal = turn;
    }


}
