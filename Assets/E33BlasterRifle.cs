using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Overheater")]
    public bool useOverheat;
    public float shotHeat;
    public float decelRate;
    public float currentHeat;
    public Image heatSlider;
    public bool isOverheated;
    bool canDecel;

    public AudioSource emptySFX, overheatSFX;
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
            if (!isOverheated)
            {
                FireGun();
            }
            else
			{
                emptySFX.Play();
            }
        }
	}
    private void InteractableObject_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        StartCoroutine(ReleasedRoutine());
    }


	private void Update()
	{
        coll.transform.position = bodyT.TransformPoint(offset);
        if (!isOverheated && canDecel) DecelHeat();
        UpdateSlider();
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

        if (decelRoutine != null) StopCoroutine(decelRoutine);
        decelRoutine = StartCoroutine(DecelRoutine());

        UpdateHeat();
    }

    Coroutine decelRoutine;
    IEnumerator DecelRoutine()
	{
        canDecel = false;
        yield return new WaitForSeconds(0.275f);
        canDecel = true;
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


    void UpdateHeat()
	{
        if (!useOverheat) return;

        currentHeat = Mathf.Clamp(currentHeat + shotHeat, 0, 1);
        if (currentHeat >= 1) StartCoroutine(OverHeat());
    }

    void DecelHeat()
	{
        currentHeat = Mathf.Clamp(currentHeat - decelRate * Time.deltaTime, 0, 1);
    }

    void UpdateSlider()
	{
        heatSlider.fillAmount = Mathf.Lerp(0.25f, 1, currentHeat);
	}


    IEnumerator OverHeat()
	{
        isOverheated = true;
        overheatSFX.Play();
        float overHeatTime = 2.5f;
        yield return new WaitForSeconds(1);
        float curTime = 0;
        while (curTime < overHeatTime)
        {
            curTime += Time.deltaTime;
            currentHeat = 1 - curTime / overHeatTime;
            UpdateSlider();
            yield return null;
        
        }
        isOverheated = false;
	}


}
