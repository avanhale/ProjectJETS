using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    AsteroidField field;
	private void Awake()
	{
        field = GetComponentInParent<AsteroidField>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        RazorCrest rc = collision.collider.GetComponentInParent<RazorCrest>();
        if (rc)
        {
            field.AsteroidHit(collision.contacts[0].point);
        }
    }


}
