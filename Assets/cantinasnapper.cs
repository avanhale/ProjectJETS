using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class cantinasnapper : MonoBehaviour
{
    VRTK_SnapDropZone zone;
    bool pickedup;
	private void Awake()
	{
        zone = GetComponent<VRTK_SnapDropZone>();
	}

	private void OnEnable()
	{
		zone.ObjectSnappedToDropZone += Zone_ObjectSnappedToDropZone;
	}
	private void OnDisable()
	{
		zone.ObjectSnappedToDropZone -= Zone_ObjectSnappedToDropZone;
	}

	private void Zone_ObjectSnappedToDropZone(object sender, SnapDropZoneEventArgs e)
	{
		if (!pickedup)
		{
			BoKatanHelmet.instance.DisableGrabbing();
			GameManager.instance.CantinaEvent2();
			pickedup = true;
		}
	}
}
