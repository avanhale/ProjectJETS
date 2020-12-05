using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterBullet : MonoBehaviour
{
    public float speed;
    public float bulletLength;
    public int damage;
    public GameObject bulletGO, hitPXGO;

    public bool isMoving;

	private void Awake()
	{
        isMoving = true;
        hitPXGO.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position += transform.forward * speed * Time.deltaTime;

        }
    }

	private void FixedUpdate()
	{
        if (!isMoving) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, bulletLength))
		{
            StormTrooper trooper = hit.collider.GetComponentInParent<StormTrooper>();
            if (trooper)
            {
                print("hit trooper");
                trooper.Damage(damage);
                StartCoroutine(HitTargetRoutine(hit.point));
            }
        }
    }


    IEnumerator HitTargetRoutine(Vector3 hitPoint)
	{
        isMoving = false;
        bulletGO.SetActive(false);
        hitPXGO.transform.position = hitPoint;
        hitPXGO.SetActive(true);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }


}
