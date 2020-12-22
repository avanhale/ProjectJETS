using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

public class TuskenBiker : MonoBehaviour
{
    public Transform bikerT;
    public float waveSpeed, waveAmp;
    public Transform headT, hipsT, speederT;
    Transform playerHeadT;
    public int clampX, clampY;
    float startHipsY;
    float curTime;
    public bool isMoving;
    public Rigidbody raiderBody;

    [Header("BikerStats")]
    public BezierSpline bikerLine;
    public float baseSpeed;
    public float m_NormalizedT;
    public float currentSpeed;
    public float targetSpeed;
    public float acceleration;

    public AudioSource speederSource, raiderSource, deathSource, shortCircuitSFX;
    public float rayLength;
    public LayerMask terrainMask;
    Rigidbody body;

    [Header("BikerSpeedPoints")]
    public Vector2[] speedPoints;
    public int currentSpeedPointIndex;

    public Collider bikeCol, bodyCol;
    public GameObject hitPXGO, shockPXGO;

    public GameObject bulletPrefab;
    public Transform bulletPointT;
    public AudioSource fireSource, crashSource;

    float startHipX;
    public bool isCaver;
    public float lineShotStep = 0.005f;

    public int numHitsNeeded;

    private void Awake()
	{
        playerHeadT = PlaySpaceRelativity.cameraT;
        startHipsY = hipsT.localPosition.y;
        body = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(bikeCol, bodyCol, true);
        hitPXGO.SetActive(false);
        shockPXGO.SetActive(false);
        startHipX = hipsT.localEulerAngles.x;
    }

	private void Start()
	{
        transform.position = bikerLine.GetPoint(0);
    }

    public void StartMoving()
	{
        isMoving = true;
        speederSource.Play();
        raiderSource.Play();
        targetSpeed = baseSpeed;
    }

    Vector3? currentGroundPos;
    Vector3? currentGroundNorm;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) if (bulletPrefab != null) Fire();
        if (!isMoving) return;


        // Handle Speed
        if (currentSpeedPointIndex < speedPoints.Length)
		{
            Vector2 nextSpeedPoint = speedPoints[currentSpeedPointIndex];
            float nextSpeedT = nextSpeedPoint.x;
            float nextSpeed = nextSpeedPoint.y;
            if (nextSpeed == -1) nextSpeed = baseSpeed;
            if (m_NormalizedT >= nextSpeedT)
            {
                targetSpeed = nextSpeed;
                currentSpeedPointIndex++;
            }
        }

        if (targetSpeed >= currentSpeed)
		{
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
		{
            currentSpeed -= acceleration * Time.deltaTime;
        }


        //currentSpeed = speedCurve.Evaluate(m_NormalizedT) * baseSpeed;
        Vector3 XZpos = bikerLine.MoveAlongSpline(ref m_NormalizedT, currentSpeed * Time.deltaTime);

        Vector3 targetPos = XZpos;


        if (!currentGroundPos.HasValue)
		{
            targetPos.y = transform.position.y;
            //targetPos.y = currentGroundPos.Value.y;// + currentGroundNorm.Value.normalized * speederGroundOffset;
		}
        else
		{
            //targetPos.y = transform.position.y;
        }


        targetPos.y = transform.position.y;



        transform.position = isCaver ? XZpos : targetPos;


        Vector3 angles = transform.localEulerAngles;
        Vector3 lineForward = bikerLine.GetTangent(m_NormalizedT);
        if (isCaver)
        {
            BezierSpline.PointIndexTuple pointIndexTuple = bikerLine.GetNearestPointIndicesTo(m_NormalizedT);
            Vector3 lineUp = Vector3.Lerp(
                bikerLine[pointIndexTuple.index1].rotation * Vector3.up,
            bikerLine[pointIndexTuple.index2].rotation * Vector3.up,
            pointIndexTuple.t);
            transform.rotation = Quaternion.LookRotation(lineForward, lineUp);
        }
        else
        {
            transform.forward = lineForward;
            transform.localEulerAngles = transform.localEulerAngles.WithX(angles.x);
        }



    }

	private void FixedUpdate()
	{
        if (!isMoving || isCaver) return;

        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength, terrainMask))
		{
           // body.useGravity = false;
            currentGroundPos = hit.point;
            currentGroundNorm = hit.normal;
        }
        else
		{
            currentGroundPos = currentGroundNorm = null;
            body.useGravity = true;
		}
	}



	private void LateUpdate()
    {
        if (!isMoving) return;
        curTime += Time.deltaTime;
        Vector3 pos = bikerT.transform.localPosition;
        pos.y = Mathf.Sin(curTime * waveSpeed) * waveAmp;
        bikerT.transform.localPosition = pos;

        headT.forward = playerHeadT.position - headT.position;
        Vector3 angles = headT.localEulerAngles;
        if (angles.x > 180) angles.x -= 360;
        if (angles.y > 180) angles.y -= 360;
        angles.x = Mathf.Clamp(angles.x, -clampX, clampX);
        angles.y = Mathf.Clamp(angles.y, -clampY, clampY);
        angles.z = 0;
        headT.localEulerAngles = angles;


        Vector3 headpos = headT.localPosition;
        headpos.y = Mathf.Sin(curTime * waveSpeed) * -0.005f;
        headT.localPosition = headpos;

        Vector3 posbody = hipsT.localPosition;
        posbody.y = startHipsY + Mathf.Sin(curTime * waveSpeed) * -0.0075f;
        hipsT.localPosition = posbody;

		Vector3 bikeAngles = speederT.localEulerAngles;
		bikeAngles.x = Mathf.Sin(curTime * waveSpeed * 2) * -.5f;
		speederT.localEulerAngles = bikeAngles;

		Vector3 hipAngles = hipsT.localEulerAngles;
        hipAngles.x = startHipX + Mathf.Sin(curTime * waveSpeed * 2) * -.5f;
        hipsT.localEulerAngles = hipAngles;

        bool added = false;
        float angleX = transform.localEulerAngles.x;
        if (angleX > 180)
		{
            added = true;
            angleX -= 360;
        }
        angleX = Mathf.Clamp(angleX, -60, 60);
        if (added) angleX += 360;
        transform.localEulerAngles = transform.localEulerAngles.WithX(angleX);

    }


    [ContextMenu("Throw")]
    public void Throw()
	{
        StartCoroutine(ThrowRoutine());
	}

    IEnumerator ThrowRoutine()
	{
        isMoving = false;
        body.constraints = RigidbodyConstraints.None;
        yield return null;
        body.velocity = transform.forward * currentSpeed;
        body.angularVelocity = transform.right * currentSpeed;

        raiderBody.transform.SetParent(null);
        raiderBody.isKinematic = false;
        raiderBody.useGravity = true;
        raiderBody.velocity = transform.forward * currentSpeed/2 + Vector3.up * currentSpeed / 3;
        raiderBody.angularVelocity = transform.right * -currentSpeed/4;

        //Physics.IgnoreCollision(bikeCol, bodyCol, false);

    }


    int numHits;
    public void Damage(int damage, Vector3 hitPoint)
	{
        numHits++;
        if (numHits == numHitsNeeded)
		{
            StartCoroutine(HitRoutine(hitPoint));
            Throw();
            deathSource.Play();
            shortCircuitSFX.Play();
            shockPXGO.SetActive(true);
        }
    }

    IEnumerator HitRoutine(Vector3 hitPoint)
	{
        hitPXGO.SetActive(true);
        hitPXGO.transform.position = hitPoint;
        //hitPXGO.transform.SetParent(null);
        yield return new WaitForSeconds(5);
        Destroy(hitPXGO);
    }



    [ContextMenu("Fire")]
    public void Fire()
	{
        StartCoroutine(FireRoutine(true));
	}

    public void FireAtPlayer()
	{
        StartCoroutine(FireRoutine(true));
    }

    IEnumerator FireRoutine(bool fireAtPlayer)
	{
        fireSource.pitch = Random.Range(0.8f, 1.2f);
        GameObject bullet = Instantiate(bulletPrefab, GameObject.Find("Bullets").transform);

        bullet.transform.position = bulletPointT.position;
        Vector3 forward = Vector3.ProjectOnPlane(bulletPointT.forward, Vector3.up);

        if (fireAtPlayer)
        {
            Vector3 playerNextPos = PodRacer.instance.racerLine.GetPoint(PodRacer.instance.m_NormalizedT + lineShotStep);
            forward = (playerNextPos - bulletPointT.position).normalized;
        }

        bullet.transform.forward = forward;

        fireSource.Play();

        yield return null;
	}


	private void OnCollisionEnter(Collision collision)
	{
        if (collision.collider.CompareTag("PodRacer"))
		{
            if (crashSource.isPlaying) return;
            crashSource.transform.position = collision.contacts[0].point;
            crashSource.Play();
		}
	}




    public void Destroy()
	{
        Destroy(gameObject);
	}





}
