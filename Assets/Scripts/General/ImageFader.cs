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
		[SerializeField] GameplayCoreRefHolder gcRef;

		public void StartFade(float target)
		{
			StartCoroutine(Fade(target));
		}

		private IEnumerator Fade(float targetAlpha)
		{
			while (!Mathf.Approximately(gcRef.gameplayCanvasGroup.alpha, targetAlpha))
			{
				//mathf.movetowards to get float to desired value using desired speed
				gcRef.gameplayCanvasGroup.alpha = 
					Mathf.MoveTowards(gcRef.gameplayCanvasGroup.alpha, 
					targetAlpha, Time.deltaTime / fadeTime);
				yield return null;
			}
		}
	}
}
