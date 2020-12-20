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
    public GameObject light01, light02;
    VRTK_BodyPhysics bodyPhysics;
    public BezierSpline racerLine;
    public AudioSource jetsSource, jetsSource2, engineSource;
    VRTK_InteractableObject interactableObject;

    public bool isMoving;
    public float m_NormalizedT;
    public float baseSpeed;

    public float rotationSpeed;
    public Transform podT;

	private void Awake()
	{
        bodyPhysics = FindObjectOfType<VRTK_BodyPhysics>();
        interactableObject = GetComponent<VRTK_InteractableObject>();
    }


	private void OnEnable()
	{
		interactableObject.InteractableObjectUsed += InteractableObject_InteractableObjectUsed;
    }
    private void OnDisable()
    {
        interactableObject.InteractableObjectUsed -= InteractableObject_InteractableObjectUsed;
    }

    private void InteractableObject_InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
	{
        if (isMoving) return;
        
        EnterDriving();
        PodEventManager.instance.StartRacing();
	}

	void Start()
    {
        light01.SetActive(false);
        light02.SetActive(false);
        ActivateJets(false);
        EnterDriving();
        PodEventManager.instance.StartRacing();
    }

    void Update()
    {
        if (isMoving)
		{
            Vector3 linePos = racerLine.MoveAlongSpline(ref m_NormalizedT, baseSpeed * Time.deltaTime);
            transform.position = linePos;

            Vector3 lineForward = racerLine.GetTangent(m_NormalizedT);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane(lineForward, Vector3.up)), rotationSpeed);

            float rotNormT = Mathf.Clamp(m_NormalizedT + 0.01f, 0, 1);
            Vector3 rotForward = racerLine.GetTangent(rotNormT);
            podT.forward = rotForward;
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
        light01.SetActive(true);
        light02.SetActive(true);
        isDriving = true;
        jetsSource.Play();
        jetsSource2.Play();
        engineSource.Play();
        Transform babyT = BabyYoda.instance.transform;
        BabyYoda.instance.ActivateCarriage();
        babyT.SetParent(seat2T);
        babyT.localPosition = babyT.localEulerAngles = Vector3.zero;
        interactableObject.isUsable = false;
        GetComponent<Collider>().enabled = false;
        isMoving = true;
    }




    void ActivateJets(bool activate = true)
	{
        jet01.SetActive(activate);
        jet02.SetActive(activate);
	}




}
