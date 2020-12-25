using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class gogo : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CanvasGroup group;

	private void Start()
	{
		Invoke("Gogo", 10);
		Invoke("fada", 15);
	}




	void Gogo()
	{
		text.DOFade(1, 2);
	}


	void fada()
	{
		DOTween.To(() => group.alpha, x => group.alpha = x, 0, 2);
	}
}
