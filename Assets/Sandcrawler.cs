using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandcrawler : MonoBehaviour
{
    public float moveSpeed;
	public Transform tracksT;
    Vector3 startPos;
    Animator anim;

    public bool isMoving;

	private void Awake()
	{
        anim = GetComponentInChildren<Animator>();
        //Close();

    }

	void Start()
    {
        startPos = tracksT.localPosition;
    }

    void Update()
    {
        if (!isMoving) return;


        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        Vector3 pos = tracksT.localPosition;
        pos.z = startPos.z + Mathf.Sin(Time.time * 2) * 0.25f;
        tracksT.localPosition = pos;
    }

    public void StartMoving()
	{
        isMoving = true;
	}

    [ContextMenu("OPen")]
    public void Open()
	{
        isMoving = false;
        anim.SetBool("isClosed", false);
    }

    [ContextMenu("Close")]
    public void Close()
	{
        anim.SetBool("isClosed", true);
        Invoke("StartMoving", 5);
    }



}
