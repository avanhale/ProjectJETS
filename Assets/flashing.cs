using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashing : MonoBehaviour
{
    public Material mat;
    float startA;
    // Start is called before the first frame update
    void Start()
    {
        startA = mat.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        Color c = mat.color;
        c.a = startA + Mathf.Sin(Time.time) * 0.05f;
        mat.color = c;
    }
}
