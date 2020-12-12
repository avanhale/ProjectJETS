using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

	private void Awake()
	{
        instance = this;
	}





	public void HyperSpaceHit()
	{
		print("HyperSpace!");
		VRTK_HeadsetFade.instance.Fade(Color.white, 5);
		AudioManager_JT.instance.HyperSpace();
	}
}
