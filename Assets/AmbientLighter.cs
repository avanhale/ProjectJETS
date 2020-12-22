using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AmbientLighter : MonoBehaviour
{
	public static AmbientLighter instance;
	Color normalColor;
	public Color darkColor;
	private void Awake()
	{
		instance = this;
		normalColor = RenderSettings.ambientLight;
	}

	[ContextMenu("Normal")]
	public void Light()
	{
		DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, normalColor, 2);
	}

	[ContextMenu("Dark")]
	public void Dark()
	{
		DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, darkColor, 2);
	}

}
