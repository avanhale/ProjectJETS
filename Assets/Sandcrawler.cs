using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

public class Sandcrawler : MonoBehaviour
{
    public float moveSpeed;
	public Transform tracksT;
    Vector3 startPos;
    Animator anim;
    BezierWalkerWithSpeed walker;

    public bool isMoving;
    public float lerpStartPodRacer;

    public Transform crawlerPointT;

	private void Awake()
	{
        anim = GetComponentInChildren<Animator>();
        walker = GetComponent<BezierWalkerWithSpeed>();
        Close();

    }

	void Start()
    {
        startPos = tracksT.localPosition;
    }

    void Update()
    {

        if (isMoving)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        if (isMoving || isTracking)
        {
            Vector3 pos = tracksT.localPosition;
            pos.z = startPos.z + Mathf.Sin(Time.time * 2) * 0.25f;
            tracksT.localPosition = pos;
        }





        if (isTracking)
		{
            if (Vector3.Distance(transform.position, crawlerPointT.position) > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, crawlerPointT.position, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, crawlerPointT.rotation, 2 * Time.deltaTime);
            }
            else
            {
                isTracking = false;
                Open();
            }
		}



    }

    public void StartMoving()
	{
        isMoving = true;
	}

    [ContextMenu("OPen")]
    public void Open()
	{
        isMoving = false;
        anim.SetBool("isClosed", false);
    }

    [ContextMenu("Close")]
    public void Close()
	{
        anim.SetBool("isClosed", true);
    }


    bool isParented;
	private void OnTriggerEnter(Collider other)
	{
        OVRCameraRig player = other.GetComponentInParent<OVRCameraRig>();
        if (player && !isParented)
		{
            player.transform.SetParent(transform);
            isParented = true;
            //VRTKCustom_Haptics.instance.SandcrawlerPulse();
        }
	}

    private void OnTriggerExit(Collider other)
    {
        OVRCameraRig player = other.GetComponentInParent<OVRCameraRig>();
        if (player && isParented)
        {
            player.transform.SetParent(null, true);
            isParented = false;
            //VRTKCustom_Haptics.instance.StopPulsing();
        }
    }

    public void StartTrack()
	{
        walker.NormalizedT = lerpStartPodRacer;
        walker.enabled = true;
        isMoving = false;
    }

    bool isTracking;
    public void StopTrack()
	{
        walker.enabled = false;
        isMoving = false;
        isTracking = true;
    }


}
