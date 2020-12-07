using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Camera mirrorCam;
    public Transform playerT;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (mirrorCam.enabled == false)
		{
            if (Vector3.Distance(playerT.position, transform.position) < 5)
                mirrorCam.enabled = true;
		}
        else
		{
            if (Vector3.Distance(playerT.position, transform.position) > 5)
                mirrorCam.enabled = false;
        }
    }
}
