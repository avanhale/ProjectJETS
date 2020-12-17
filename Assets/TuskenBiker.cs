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

    [Header("BikerStats")]
    public BezierSpline bikerLine;
    public float baseSpeed;
    public AnimationCurve speedCurve;
    public float m_NormalizedT;
    public float currentSpeed;
    public float targetSpeed;
    public float acceleration;

    public AudioSource speederSource, raiderSource;
    public float rayLength;
    public LayerMask terrainMask;
    Rigidbody body;

    [Header("BikerSpeedPoints")]
    public Vector2[] speedPoints;
    public int currentSpeedPointIndex;

    private void Awake()
	{
        playerHeadT = PlaySpaceRelativity.cameraT;
        startHipsY = hipsT.localPosition.y;
        body = GetComponent<Rigidbody>();
        Invoke("StartMoving", Random.Range(1, 3));
    }

    void StartMoving()
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



        transform.position = targetPos;


        Vector3 angles = transform.localEulerAngles;
        Vector3 lineForward = bikerLine.GetTangent(m_NormalizedT);
        transform.forward = lineForward;
        transform.localEulerAngles = transform.localEulerAngles.WithX(angles.x);

        //transform.rotation = Quaternion.LookRotation(bikerLine.GetTangent(m_NormalizedT));



    }

	private void FixedUpdate()
	{
        if (!isMoving) return;

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
        bikeAngles.x = Mathf.Sin(curTime * waveSpeed *2) * -.5f;
        speederT.localEulerAngles = bikeAngles;

        Vector3 hipAngles = hipsT.localEulerAngles;
        hipAngles.x = Mathf.Sin(curTime * waveSpeed * 2) * -.5f;
        hipsT.localEulerAngles = hipAngles;


    }







    public void Damage(int damage)
	{

	}





}
