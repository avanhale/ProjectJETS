using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerBody : MonoBehaviour
{
    public Transform contL, contR, headT;
    public Transform headBoneT;
    public float backOffset, veticalOffset;
    public float rotationSpeed, positionSpeed;
    Animator animator;
    VRTK_SlideObjectControlAction[] sliders;
    public LayerMask airMask;
    public bool isMidAir;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sliders = FindObjectsOfType<VRTK_SlideObjectControlAction>();
    }

    void Update()
    {
        UpdateRotation();
        UpdatePosition();

        TryUpdateAnim();

        FollowHead();
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(headT.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3, airMask, QueryTriggerInteraction.Ignore))
        {
            animator.SetBool("MidAir", false);
            isMidAir = false;
        }
        else
        {
            animator.SetBool("MidAir", true);
            isMidAir = true;
        }
    }


    void UpdateRotation()
    {
        Vector3 fwdHead = Vector3.ProjectOnPlane(headT.forward, Vector3.up);
        Vector3 crossFwd = Vector3.Cross(contR.position - contL.position, Vector3.up);
        //if (Vector3.Dot(crossFwd, headT.forward) < 0) crossFwd *= -1;
        Quaternion yRot = Quaternion.LookRotation(crossFwd, Vector3.up);
        if (Vector3.Angle(fwdHead, yRot * Vector3.forward) > 85) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, yRot, rotationSpeed);
    }

    void UpdatePosition()
    {
        Vector3 lerper = Vector3.Slerp(Vector3.ProjectOnPlane(headT.forward, Vector3.up).normalized, Vector3.ProjectOnPlane(transform.rotation * Vector3.forward, Vector3.up), 0.33f);
        Vector3 pos = headT.position + -lerper.normalized * backOffset;
        pos += Vector3.down * veticalOffset;
        transform.position = Vector3.Lerp(transform.position, pos, positionSpeed);
    }

    void TryUpdateAnim()
    {
        bool isMoving = sliders[0].isMoving ? true : sliders[1].isMoving ? true : false;
        animator.SetBool("Walking", isMoving);
    }

    void FollowHead()
    {
        //headBoneT.position = headT.position;
        headBoneT.rotation = headT.rotation;
    }

}
