using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHead : MonoBehaviour
{
    public float posLerp, rotLerp;
    public Transform headT;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, headT.position, posLerp);
        transform.rotation = Quaternion.Slerp(transform.rotation, headT.rotation, rotLerp);
    }
}
