using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickeringLight : MonoBehaviour
{
    Light light;
    float startIntensity;
    public float intesityAmp;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        startIntensity = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity = startIntensity + Mathf.Sin(Time.time) * intesityAmp;
    }
}
