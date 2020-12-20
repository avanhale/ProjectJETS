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
    public Collider coll;
    public Transform bodyT;
    public Vector3 offset;
    Light blastLight;

	private void Awake()
	{
        interactableObject = GetComponent<VRTK_InteractableObject>();
        fireSource = GetComponentInChildren<AudioSource>();
        bulletsT = GameObject.Find("Bullets").transform;
        blastLight = GetComponentInChildren<Light>();
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


	private void Update()
	{
        coll.transform.position = bodyT.TransformPoint(offset);
    }

	void FireGun()
	{
        fireSource.pitch = Random.Range(0.85f, 1.15f);
        fireSource.Play();
        GameObject bullet = Instantiate(blasterBulletPrefab, bulletsT, false);
        bullet.transform.position = bulletPointT.position;
        bullet.transform.rotation = bulletPointT.rotation;

        VRTKCustom_Haptics.instance.BlasterShot();
        StartCoroutine(StartLight());
    }


    IEnumerator ReleasedRoutine()
	{
        yield return new WaitForEndOfFrame();
        dropZone.ForceSnap(gameObject);

    }


    IEnumerator StartLight()
	{
        blastLight.enabled = true;
        yield return new WaitForSeconds(0.05f);
        blastLight.enabled = false;
    }

}
