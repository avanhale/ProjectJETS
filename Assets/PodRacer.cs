using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using BezierSolution;

public class PodRacer : MonoBehaviour
{
    public bool isDriving;
    public Transform playerT, drivingSeatT, seat2T;
    public GameObject jet01, jet02;
    VRTK_BodyPhysics bodyPhysics;
    BezierWalkerWithSpeed splineWalker;
    public AudioSource jetsSource, jetsSource2, engineSource;

	private void Awake()
	{
        bodyPhysics = FindObjectOfType<VRTK_BodyPhysics>();
        splineWalker = GetComponent<BezierWalkerWithSpeed>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (splineWalker.enabled)
		{
            //bodyPhysics.transform.localPosition = bodyPhysics.transform.localPosition.WithY(Mathf.Sin(Time.time) * 0.05f);

        }
    }

    [ContextMenu("EnterDriving")]
    public void EnterDriving()
	{
        StartCoroutine(EnterDrivingSeatRoutine());
    }

    IEnumerator EnterDrivingSeatRoutine()
    {
        VRTK_HeadsetFade.instance.Fade(Color.black, 1);
        yield return new WaitForSeconds(1);
        bodyPhysics.transform.SetParent(drivingSeatT, false);
        PlaySpaceRelativity.TransformCameraTo(drivingSeatT);
        bodyPhysics.enableBodyCollisions = false;
        VRTK_HeadsetFade.instance.Unfade(1);
        FindObjectOfType<VRTK_SlideObjectControlAction>().gameObject.SetActive(false);
        ActivateJets();
        isDriving = true;
        splineWalker.enabled = true;
        jetsSource.Play();
        jetsSource2.Play();
        engineSource.Play();
        Transform babyT = BabyYoda.instance.transform;
        BabyYoda.instance.ActivateCarriage();
        babyT.SetParent(seat2T);
        babyT.localPosition = babyT.localEulerAngles = Vector3.zero;

    }




    void ActivateJets(bool activate = true)
	{
        jet01.SetActive(activate);
        jet02.SetActive(activate);
	}




}
