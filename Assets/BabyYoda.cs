using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BabyYoda : MonoBehaviour
{
    public static BabyYoda instance;
    public Transform headT;
    public Transform playerHeadT;
    VRTK_InteractableObject interactableObject;
    public int clampX, clampY;
    public GameObject carriageGO;
    Animator animator;

    public AudioClip cooing, fussing;
    public AudioSource mouthSource;

    private void Awake()
    {
        instance = this;
        interactableObject = GetComponent<VRTK_InteractableObject>();
        animator = GetComponentInChildren<Animator>();
        Cooing();
    }

    private void OnEnable()
    {
        interactableObject.InteractableObjectGrabbed += InteractableObject_InteractableObjectGrabbed;
        interactableObject.InteractableObjectUngrabbed += InteractableObject_InteractableObjectUngrabbed; ;
    }

    private void InteractableObject_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        animator.SetBool("Floating", true);
    }

    private void InteractableObject_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        animator.SetBool("Floating", false);
    }

    void Start()
    {
        ActivateCarriage(false);
    }



    private void LateUpdate()
    {
        if (interactableObject.IsGrabbed())
        {
            headT.forward = playerHeadT.position - headT.position;
            Vector3 angles = headT.localEulerAngles;
            if (angles.x > 180) angles.x -= 360;
            if (angles.y > 180) angles.y -= 360;
            angles.x = Mathf.Clamp(angles.x, -clampX, clampX);
            angles.y = Mathf.Clamp(angles.y, -clampY, clampY);
            angles.z = 0;
            headT.localEulerAngles = angles;
        }
    }

    [ContextMenu("ActivateCarriage")]
    public void GO()
    {
        ActivateCarriage();
    }
    public void ActivateCarriage(bool activate = true)
    {
        carriageGO.SetActive(activate);
        animator.SetBool("Floating", activate);

    }


    public void Cooing()
	{
        mouthSource.clip = cooing;
        mouthSource.Play();
    }

    public void Fussing()
    {
        mouthSource.clip = fussing;
        mouthSource.Play();
    }

}
