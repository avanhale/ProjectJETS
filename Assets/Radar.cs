using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Radar : MonoBehaviour
{
    Transform rc;
    public RectTransform radarDot;
    public Transform targetSpot;
	private void Awake()
	{
        rc = GetComponentInParent<RazorCrest>().transform;

    }
	// Start is called before the first frame update
	void Start()
    {
        Color c = radarDot.GetComponent<Image>().color;
        Image image = radarDot.GetComponent<Image>();
        image.DOColor(c * 0.5f, 1.5f).SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = targetSpot.position - rc.position;
        Vector3 projection = Vector3.ProjectOnPlane(direction, transform.forward);
        projection = Quaternion.AngleAxis(-55, transform.forward) * projection;
        radarDot.position = transform.position + projection.normalized * 0.1f;
    }
}
