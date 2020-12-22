using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BikerEvents : MonoBehaviour
{
    TuskenBiker biker;

    [System.Serializable]
    public class Event
	{
        public float timeStamp;
		public bool passed;
        public UnityEvent targetEvent;
    }
    public Event[] events;

	private void Awake()
	{
        biker = GetComponent<TuskenBiker>();
	}


	private void Update()
	{
		foreach (var ev in events)
		{
			if (!ev.passed)
			{
				if (biker.m_NormalizedT >= ev.timeStamp)
				{
					ev.targetEvent?.Invoke();
					ev.passed = true;
					break;
				}
			}
		}
	}



	public void TimeSplit01()
	{
		TimeSplitter.instance.TimeSplit(0.5f, 1.25f);
	}



	public void HitPlayer()
	{
		GameManager.instance.HitIndication();
	}


	public void DamagePlayer()
	{
		GameManager.instance.HitIndication();
		PodRacer.instance.LoseEngine();
	}



}
