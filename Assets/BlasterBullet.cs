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
    public bool isTusken;
    public bool isTrooper;
    StormTrooper trooper;
    Light light;

	private void Awake()
	{
        isMoving = true;
        hitPXGO.SetActive(false);
        light = GetComponentInChildren<Light>();
    }

    public void SetTrooper(StormTrooper st)
	{
        trooper = st;
	}

	private void Start()
	{
        if (!isTusken) ToBlaster(true);
        Invoke("Destroy", 5);
    }

    bool hasMovedU, hasMovedS;
	void Update()
    {
        if (isMoving)
        {
            if (!hasMovedU && !isTusken)
			{
                ToBlaster(false);
			}
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    void ToBlaster(bool isS)
	{
        if (isTrooper)
		{
            transform.position = trooper.gunPointT.position;
            transform.rotation = trooper.gunPointT.rotation;
        }
        else
		{
            E33BlasterRifle blaster = FindObjectOfType<E33BlasterRifle>();
            transform.position = blaster.bulletPointT.position;
            transform.rotation = blaster.bulletPointT.rotation;
        }

        if (isS) hasMovedS = true; else hasMovedU = true;

    }

    private void FixedUpdate()
	{
        if (!isMoving || isTusken) return;

        Ray ray = new Ray(transform.position - transform.forward, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, bulletLength))
		{

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NavMesh")
                || hit.collider.CompareTag("Rock")
                || hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
			{
                Destroy();
                return;
			}




            if (!isTrooper)
            {
                StormTrooper trooper = hit.collider.GetComponentInParent<StormTrooper>();
                if (trooper)
                {
                    print("hit trooper");
                    trooper.Damage(damage, hit.point);
                    StartCoroutine(HitTargetRoutine(hit.point));
                }
            }
            
            if (hit.collider.CompareTag("Biker"))
            {
                TuskenBiker tuskenBiker = hit.collider.GetComponentInParent<TuskenBiker>();
                print("hit tusken biker");
                tuskenBiker.Damage(damage, hit.point);
                StartCoroutine(HitTargetRoutine(hit.point));
            }

            OVRCameraRig player = hit.collider.GetComponentInParent<OVRCameraRig>();
            if (player && isTrooper)
			{
                GameManager.instance.HitIndication();
			}



            Sandworm sandworm = hit.collider.GetComponentInParent<Sandworm>();
            if (sandworm)
            {
                print("hit sand worm");
                sandworm.Damage(damage);
                StartCoroutine(HitTargetRoutine(hit.point));
            }


        }
    }


    IEnumerator HitTargetRoutine(Vector3 hitPoint)
	{
        StartCoroutine(LightHit());
        isMoving = false;
        bulletGO.SetActive(false);
        hitPXGO.transform.position = hitPoint;
        hitPXGO.SetActive(true);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    IEnumerator LightHit()
	{
        light.intensity *= 2;
        yield return new WaitForSeconds(0.05f);
        light.enabled = false;
    }


    void Destroy()
	{
        Destroy(gameObject);
    }

}
