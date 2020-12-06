using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;

public class RazorCrest : MonoBehaviour
{
    public VRTK_ControllerEvents controller;
    public VRTK_ControllerEvents.ButtonAlias actionButton = VRTK_ControllerEvents.ButtonAlias.ButtonTwoPress;
    public VRTK_ControllerEvents.ButtonAlias actionButton2 = VRTK_ControllerEvents.ButtonAlias.ButtonTwoPress;
    public VRTK_ControllerEvents.ButtonAlias actionButton3 = VRTK_ControllerEvents.ButtonAlias.ButtonTwoPress;
    public int lift, turn, forward;
    public float liftForce, turnForce, forwardForce;
    Rigidbody body;
    VRTK_BodyPhysics bodyPhysics;
    public Transform drivingSeatT;

    public VRTK_ArtificialRotator controls_Lift, controls_Speed, controls_Turn;

	private void Awake()
	{
        body = GetComponent<Rigidbody>();
        bodyPhysics = FindObjectOfType<VRTK_BodyPhysics>();
        body.maxAngularVelocity = 100f;
    }

	private void OnEnable()
    {
        controller.SubscribeToButtonAliasEvent(actionButton, true, Controller_JetPackButtonPressed);
        controller.SubscribeToButtonAliasEvent(actionButton2, true, Controller_JetPackButtonPressed2);
        controller.SubscribeToButtonAliasEvent(actionButton3, true, Controller_JetPackButtonPressed3);
		controls_Lift.ValueChanged += Controls_Lift_ValueChanged;
        controls_Speed.ValueChanged += Controls_Speed_ValueChanged;
        controls_Turn.ValueChanged += Controls_Turn_ValueChanged;
    }

	

	private void OnDisable()
    {
        controller.UnsubscribeToButtonAliasEvent(actionButton, true, Controller_JetPackButtonPressed);
        controller.SubscribeToButtonAliasEvent(actionButton2, true, Controller_JetPackButtonPressed2);
        controller.SubscribeToButtonAliasEvent(actionButton3, true, Controller_JetPackButtonPressed3);
        controls_Lift.ValueChanged -= Controls_Lift_ValueChanged;
        controls_Speed.ValueChanged -= Controls_Speed_ValueChanged;
        controls_Turn.ValueChanged += Controls_Turn_ValueChanged;
    }

    private void Controller_JetPackButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        ToggleLift();
    }
    private void Controller_JetPackButtonPressed2(object sender, ControllerInteractionEventArgs e)
    {
        ToggleTurn();
    }
    private void Controller_JetPackButtonPressed3(object sender, ControllerInteractionEventArgs e)
    {
        ToggleForward();
    }

    private void Controls_Lift_ValueChanged(object sender, VRTK.Controllables.ControllableEventArgs e)
    {
        lift = (((int) e.value)-1)*-1;
    }
    private void Controls_Speed_ValueChanged(object sender, VRTK.Controllables.ControllableEventArgs e)
    {
        forward = ((int)e.value);
    }
    private void Controls_Turn_ValueChanged(object sender, VRTK.Controllables.ControllableEventArgs e)
    {
        turn = ((int)e.value)-1;
    }

    private void FixedUpdate()
	{
        // Lift
        body.AddForce(Vector3.up * liftForce * lift);

        // Turn
        body.AddTorque(Vector3.up * turnForce * turn);

        // Forward
        body.AddForce(transform.forward * forwardForce * forward);

    }

    private void LateUpdate()
	{
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }


    void ToggleLift()
	{
        if (lift == 1) lift = 0;
        else if (lift == 0) lift = -1;
        else lift = 1;
	}

    void ToggleTurn()
    {
        if (turn == 1) turn = 0;
        else if (turn == 0) turn = -1;
        else turn = 1;
    }

    void ToggleForward()
    {
        if (forward == 1) forward = 2;
        else if (forward == 2) forward = 0;
        else forward = 1;
    }

    [ContextMenu("Enter")]
    public void EnterDrivingSeat()
	{
        StartCoroutine(EnterDrivingSeatRoutine());
	}

    IEnumerator EnterDrivingSeatRoutine()
	{
        VRTK_HeadsetFade.instance.Fade(Color.black, 1);
        yield return new WaitForSeconds(1);
        bodyPhysics.transform.SetParent(drivingSeatT, false);
        bodyPhysics.transform.localPosition = bodyPhysics.transform.localEulerAngles = Vector3.zero;
        bodyPhysics.enableBodyCollisions = false;
        VRTK_HeadsetFade.instance.Unfade(1);
        FindObjectOfType<VRTK_SlideObjectControlAction>().gameObject.SetActive(false);

    }


}
