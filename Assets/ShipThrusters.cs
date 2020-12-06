using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThrusters : MonoBehaviour
{
    RazorCrest rc;
    JetPack jp;

    int lastForward;

	private void Awake()
	{
        rc = GetComponentInParent<RazorCrest>();
        jp = GetComponent<JetPack>();
	}

    void Update()
    {
        if (!rc.isDriving) return;

        if (rc.forward != lastForward)
		{
            if (rc.forward == 0)
                jp.EndJets();
            else
                jp.StartJets();
        }
        lastForward = rc.forward;
    }
}
