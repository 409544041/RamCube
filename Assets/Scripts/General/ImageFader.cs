using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.General
{
	public class ImageFader : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float fadeTime;

		//Cache
		CanvasGroup canvasGroup;
		FinishCube finish;

		private void Awake() 
		{
			canvasGroup = GetComponent<CanvasGroup>();	
			finish = FindObjectOfType<FinishCube>();
		}

		private void OnEnable() 
		{
			if (finish != null) finish.onUIFade += StartFade;
		}

		private void StartFade(float target)
		{
			StartCoroutine(Fade(target));
		}

		private IEnumerator Fade(float targetAlpha)
		{
			while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
			{
				//mathf.movetowards to get float to desired value using desired speed
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / fadeTime);
				yield return null;
			}
		}

		private void OnDisable()
		{
			if (finish != null) finish.onUIFade -= StartFade;
		}
	}
}
