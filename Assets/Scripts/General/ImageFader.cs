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
		[SerializeField] CanvasGroup canvasGroup;

		public void StartFade(float target)
		{
			StartCoroutine(Fade(target));
		}

		public IEnumerator Fade(float targetAlpha)
		{
			while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
			{
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 
					targetAlpha, Time.deltaTime / fadeTime);

				yield return null;
			}
		}
	}
}
