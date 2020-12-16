using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuskenBiker : MonoBehaviour
{
    public Transform bikerT;
    public float waveSpeed, waveAmp;
    public Transform headT, hipsT, speederT;
    Transform playerHeadT;
    public int clampX, clampY;

    float startHipsY;

    private void Awake()
	{
        playerHeadT = PlaySpaceRelativity.cameraT;
        startHipsY = hipsT.localPosition.y;

    }


    void Update()
    {
        Vector3 pos = bikerT.transform.localPosition;
        pos.y = Mathf.Sin(Time.time * waveSpeed) * waveAmp;
        bikerT.transform.localPosition = pos;
    }




    private void LateUpdate()
    {
        headT.forward = playerHeadT.position - headT.position;
        Vector3 angles = headT.localEulerAngles;
        if (angles.x > 180) angles.x -= 360;
        if (angles.y > 180) angles.y -= 360;
        angles.x = Mathf.Clamp(angles.x, -clampX, clampX);
        angles.y = Mathf.Clamp(angles.y, -clampY, clampY);
        angles.z = 0;
        headT.localEulerAngles = angles;


        Vector3 pos = headT.localPosition;
        pos.y = Mathf.Sin(Time.time * waveSpeed) * -0.005f;
        headT.localPosition = pos;

        Vector3 posbody = hipsT.localPosition;
        posbody.y = startHipsY + Mathf.Sin(Time.time * waveSpeed) * -0.0075f;
        hipsT.localPosition = posbody;

        Vector3 bikeAngles = speederT.localEulerAngles;
        bikeAngles.x = Mathf.Sin(Time.time * waveSpeed *2) * -.5f;
        speederT.localEulerAngles = bikeAngles;


    }



}
