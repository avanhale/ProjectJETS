using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class Targeter : MonoBehaviour
{
    public Transform impT;
    public Transform reticle;
    Vector3 targetPoint;
    MeshRenderer mesh;
    Color startCol;
    public Color red;
    public Rocket rocket;

    public VRTK_ControllerEvents controller;
    public VRTK_ControllerEvents.ButtonAlias actionButton = VRTK_ControllerEvents.ButtonAlias.ButtonTwoPress;

    void Start()
    {
        mesh = reticle.GetComponent<MeshRenderer>();
        startCol = mesh.material.color;
    }

    private void OnEnable()
    {
        controller.SubscribeToButtonAliasEvent(actionButton, true, Controller_JetPackButtonPressed);
    }
    private void OnDisable()
    {
        controller.UnsubscribeToButtonAliasEvent(actionButton, true, Controller_JetPackButtonPressed);
    }

    private void Controller_JetPackButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (isLocked && !hasFired)
		{
            rocket.Fire();
            GameManager.instance.SetProfile();
            hasFired = true;
        }
    }

    void Update()
    {
        reticle.forward = PlaySpaceRelativity.cameraT.position - reticle.position;
        reticle.position = Vector3.Lerp(reticle.position, targetPoint, 0.2f);
    }

    bool isHitting;
    bool isLocked;
    bool hasFired;
    private void FixedUpdate()
	{
        if (hasFired)
		{
            targetPoint = impT.position;
            mesh.material.color = red;
            return;
		}

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hit;
        isHitting = false;

        if (Physics.Raycast(ray, out hit, 250f))
		{
            targetPoint = ray.GetPoint(hit.distance * 0.95f);
            reticle.localScale = Vector3.one;


            if (hit.collider.CompareTag("Imp"))
			{
                targetPoint = hit.collider.transform.position;
                reticle.localScale = Vector3.one * 5;
                isHitting = true;
                mesh.material.color = red;
                isLocked = true;
            }
            else
			{
                mesh.material.color = startCol;
            }
        }
        else
		{
            targetPoint = ray.GetPoint(100f);
            reticle.localScale = Vector3.one;
            mesh.material.color = startCol;
        }
    }

    void StartChecker()
	{
        if (checkerR == null) checkerR = StartCoroutine(Checker());
	}

    Coroutine checkerR;
    IEnumerator Checker()
	{
		for (int i = 0; i < 5; i++)
		{
            yield return new WaitForSeconds(0.3f);
            if (!isHitting)
			{
                yield break;
			}
        }
        mesh.material.color = Color.red;
        isLocked = true;
    }

    void GoRed()
	{
        if (isHitting)
		{
            mesh.material.color = Color.red;
        }
	}


}
