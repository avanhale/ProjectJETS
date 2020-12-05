using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class JetPack : MonoBehaviour
{
    public VRTK_ControllerEvents controller;
    public VRTK_ControllerEvents.ButtonAlias actionButton = VRTK_ControllerEvents.ButtonAlias.ButtonTwoPress;
    VRTK_BodyPhysics body;

    public AudioClip jetStart, jetMid, jetEnd;
    AudioSource source;


    public float jetForce;

    public bool isJetting;

	private void Awake()
	{
        body = FindObjectOfType<VRTK_BodyPhysics>();
        source = GetComponent<AudioSource>();
    }

	private void OnEnable()
	{
        controller.SubscribeToButtonAliasEvent(actionButton, true, Controller_JetPackButtonPressed);
        controller.SubscribeToButtonAliasEvent(actionButton, false, Controller_JetPackButtonUnPressed);
    }
    private void OnDisable()
    {
        controller.UnsubscribeToButtonAliasEvent(actionButton, true, Controller_JetPackButtonPressed);
        controller.UnsubscribeToButtonAliasEvent(actionButton, false, Controller_JetPackButtonUnPressed);
    }
    private void Controller_JetPackButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        ToggleJets();
    }

    private void Controller_JetPackButtonUnPressed(object sender, ControllerInteractionEventArgs e)
    {

    }

    void Update()
    {
        if (isJetting)
		{
            body.ApplyBodyVelocity(Vector3.up * jetForce, true);

        }
    }

    void ToggleJets()
	{
        if (isJetting) EndJets();
        else StartJets();
	}

    void StartJets()
	{
        isJetting = true;
        if (startJetsRoutine != null) StopCoroutine(startJetsRoutine);
        startJetsRoutine = StartCoroutine(StartJetsRoutine());
    }

    Coroutine startJetsRoutine;
    IEnumerator StartJetsRoutine()
	{
        source.clip = jetStart;
        source.loop = false;
        source.Play();
        yield return new WaitForSeconds(jetStart.length);
        if (isJetting)
		{
            source.clip = jetMid;
            source.Play();
            source.loop = true;
		}
	}

    void EndJets()
	{
        isJetting = false;
        source.clip = jetEnd;
        source.loop = false;
        source.Play();
    }



}
