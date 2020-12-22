using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class JetPack : MonoBehaviour
{
    public static JetPack instance;
    public VRTK_ControllerEvents controller;
    public VRTK_ControllerEvents.ButtonAlias actionButton = VRTK_ControllerEvents.ButtonAlias.ButtonTwoPress;
    VRTK_BodyPhysics body;

    public AudioClip jetStart, jetMid, jetEnd;
    AudioSource source;

    public bool isShipJets;
    public float jetForce;

    public bool isJetting;
    public bool canJets;

    public delegate void JetPackPressed();
    public static event JetPackPressed OnJetPackPressed;

    private void Awake()
	{
        if (!isShipJets) instance = this;
        body = FindObjectOfType<VRTK_BodyPhysics>();
        source = GetComponent<AudioSource>();
        canJets = true;
    }

	private void OnEnable()
	{
        if (isShipJets) return;
        controller.SubscribeToButtonAliasEvent(actionButton, true, Controller_JetPackButtonPressed);
        controller.SubscribeToButtonAliasEvent(actionButton, false, Controller_JetPackButtonUnPressed);
    }
    private void OnDisable()
    {
        if (isShipJets) return;

        controller.UnsubscribeToButtonAliasEvent(actionButton, true, Controller_JetPackButtonPressed);
        controller.UnsubscribeToButtonAliasEvent(actionButton, false, Controller_JetPackButtonUnPressed);
    }
    private void Controller_JetPackButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (canJets) ToggleJets();
        OnJetPackPressed?.Invoke();
    }

    private void Controller_JetPackButtonUnPressed(object sender, ControllerInteractionEventArgs e)
    {

    }

    void Update()
    {
        if (isShipJets || tempSpeed) return;

        if (isJetting)
		{
            body.ApplyBodyVelocity(Vector3.up * jetForce, true, true);

        }
    }

    public void ToggleJets()
	{
        if (isJetting) EndJets();
        else StartJets();

    }

    public void StartJets()
	{
        isJetting = true;
        Jets();
    }

    public void Jets()
	{
        if (startJetsRoutine != null) StopCoroutine(startJetsRoutine);
        startJetsRoutine = StartCoroutine(StartJetsRoutine());
    }

    public Coroutine startJetsRoutine;
    IEnumerator StartJetsRoutine()
	{
        source.clip = jetStart;
        source.loop = false;
        source.Play();
        yield return new WaitForSeconds(jetStart.length);
        if (isJetting || !canJets)
		{
            source.clip = jetMid;
            source.Play();
            source.loop = true;
		}
	}

    public void EndJets()
	{
        isJetting = false;
        source.clip = jetEnd;
        source.loop = false;
        source.Play();
    }

    bool tempSpeed;
    public void SetTempSpeed(bool temp)
	{
        tempSpeed = temp;
    }


}
