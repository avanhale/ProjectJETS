using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class E33BlasterRifle : MonoBehaviour
{
    public VRTK_ControllerEvents controller;
    VRTK_InteractableObject interactableObject;
    AudioSource fireSource;
    public GameObject blasterBulletPrefab;
    Transform bulletsT;
    public Transform bulletPointT;

	private void Awake()
	{
        interactableObject = GetComponent<VRTK_InteractableObject>();
        fireSource = GetComponentInChildren<AudioSource>();
        bulletsT = GameObject.Find("Bullets").transform;
    }

	private void OnEnable()
	{
        controller.SubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, Controller_FireButtonPressed);
	}
    private void OnDisable()
    {
        controller.UnsubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, Controller_FireButtonPressed);
    }
    private void Controller_FireButtonPressed(object sender, ControllerInteractionEventArgs e)
	{
        if (interactableObject.IsGrabbed())
        {
            FireGun();
        }
	}



    void FireGun()
	{
        fireSource.pitch = Random.Range(0.85f, 1.15f);
        fireSource.Play();
        GameObject bullet = Instantiate(blasterBulletPrefab, bulletsT, false);
        bullet.transform.position = bulletPointT.position;
        bullet.transform.rotation = bulletPointT.rotation;
    }






}
