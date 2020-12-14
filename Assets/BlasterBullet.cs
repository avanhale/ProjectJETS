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

	private void Start()
	{
        ToBlaster(true);
    }

    bool hasMovedU, hasMovedS;
	void Update()
    {
        if (isMoving)
        {
            if (!hasMovedU)
			{
                ToBlaster(false);
			}
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    void ToBlaster(bool isS)
	{
        E33BlasterRifle blaster = FindObjectOfType<E33BlasterRifle>();
        transform.position = blaster.bulletPointT.position;
        transform.rotation = blaster.bulletPointT.rotation;
        if (isS) hasMovedS = true; else hasMovedU = true;
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
