using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BabyYoda : MonoBehaviour
{
    public Transform headT;
    public Transform playerHeadT;
    VRTK_InteractableObject interactableObject;
    public int clampX, clampY;
	private void Awake()
	{
        interactableObject = GetComponent<VRTK_InteractableObject>();

    }
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
