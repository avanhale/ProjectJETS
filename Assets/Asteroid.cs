using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    AsteroidField field;
	private void Awake()
	{
        field = GetComponentInParent<AsteroidField>();
        GetComponent<Rigidbody>().maxAngularVelocity = 100f;
	}

    private void OnCollisionEnter(Collision collision)
    {
        RazorCrest rc = collision.collider.GetComponentInParent<RazorCrest>();
        if (rc)
        {
            AudioManager_JT.instance.AsteroidMetalHit(collision.contacts[0].point);
        }
    }


}
