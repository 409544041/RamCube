using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.SceneTransition
{
	public class Fader : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float transitionTime;

		//Cache
		CanvasGroup canvasGroup = null;
		Coroutine activeCoroutine = null;

		void Awake()
		{
			canvasGroup = GetComponentInChildren<CanvasGroup>();
		}

		public void FadeOutImmediate()
		{
			canvasGroup.alpha = 1;
		}

		public Coroutine FadeOut()
		{
			return Fade(1);
		}

		public Coroutine FadeIn()
		{
			return Fade(0);
		}

		public Coroutine Fade(float target) 
		{
			if (activeCoroutine != null) StopCoroutine(activeCoroutine);
			activeCoroutine = StartCoroutine(FadeRoutine(target)); 
			return activeCoroutine;
		}

		private IEnumerator FadeRoutine(float target)
		{
			while (!Mathf.Approximately(canvasGroup.alpha, target))
			{
				//mathf.movetowards to get float to desired value using desired speed
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / transitionTime);
				yield return null;
			}
		}
	}
}
