using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lift : MonoBehaviour
{
    public bool isSpeed;
    RazorCrest razorCrest;
    public MeshRenderer UI_L, UI_R;

	private void Awake()
	{
        razorCrest = GetComponentInParent<RazorCrest>();
    }

    void Update()
    {
        UpdateUI();
    }


    void UpdateUI()
	{
        if (!isSpeed)
        {
            int lift = razorCrest.lift;
            if (lift == -1)
            {
                Turn(UI_L, false);
                Turn(UI_R, false);
            }
            else if (lift == 0)
            {
                Turn(UI_L, true);
                Turn(UI_R, false);
            }
            else if (lift == 1)
            {
                Turn(UI_L, true);
                Turn(UI_R, true);
            }
        }
        else
		{
            int speed = razorCrest.forward;
            if (speed == 0)
            {
                Turn(UI_L, false);
                Turn(UI_R, false);
            }
            else if (speed == 1)
            {
                Turn(UI_L, true);
                Turn(UI_R, false);
            }
            else if (speed == 2)
            {
                Turn(UI_L, true);
                Turn(UI_R, true);
            }
        }
    }

    void Turn(MeshRenderer light, bool on)
	{
        if (on) light.material.EnableKeyword("_EMISSION");
        else light.material.DisableKeyword("_EMISSION");
        light.material.globalIlluminationFlags = on ? MaterialGlobalIlluminationFlags.RealtimeEmissive : MaterialGlobalIlluminationFlags.EmissiveIsBlack;
	}

}
