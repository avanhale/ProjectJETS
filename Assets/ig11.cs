using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ig11 : MonoBehaviour
{
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time) * 0.015f;
    }
}
