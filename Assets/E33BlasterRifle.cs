using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class E33BlasterRifle : MonoBehaviour
{
    public VRTK_ControllerEvents controllerR, controllerL;
    VRTK_InteractableObject interactableObject;
    AudioSource fireSource;
    public GameObject blasterBulletPrefab;
    Transform bulletsT;
    public Transform bulletPointT;
    public VRTK_SnapDropZone dropZone;

	private void Awake()
	{
        interactableObject = GetComponent<VRTK_InteractableObject>();
        fireSource = GetComponentInChildren<AudioSource>();
        bulletsT = GameObject.Find("Bullets").transform;
    }

	private void OnEnable()
	{
        controllerR.SubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, Controller_FireButtonPressed);
        controllerL.SubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, Controller_FireButtonPressed);
        interactableObject.InteractableObjectUngrabbed += InteractableObject_InteractableObjectUngrabbed;

    }

	

	private void OnDisable()
    {
        controllerR.UnsubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, Controller_FireButtonPressed);
        controllerL.SubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.TriggerPress, true, Controller_FireButtonPressed);
        interactableObject.InteractableObjectUngrabbed -= InteractableObject_InteractableObjectUngrabbed;
    }
    private void Controller_FireButtonPressed(object sender, ControllerInteractionEventArgs e)
	{
        if (interactableObject.IsGrabbed())
        {
            FireGun();
        }
	}

    private void InteractableObject_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        StartCoroutine(ReleasedRoutine());
    }

    void FireGun()
	{
        fireSource.pitch = Random.Range(0.85f, 1.15f);
        fireSource.Play();
        GameObject bullet = Instantiate(blasterBulletPrefab, bulletsT, false);
        bullet.transform.position = bulletPointT.position;
        bullet.transform.rotation = bulletPointT.rotation;
    }


    IEnumerator ReleasedRoutine()
	{
        yield return new WaitForEndOfFrame();
        dropZone.ForceSnap(gameObject);

    }




}
