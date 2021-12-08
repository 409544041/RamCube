using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.SceneTransition
{
	public class Fader : MonoBehaviour
	{
		//Config parameters
		public float sceneTransTime, endSeqTransTime = .5f;

		//Cache
		CanvasGroup canvasGroup = null;
		Coroutine activeCoroutine = null;

		void Awake()
		{
			canvasGroup = GetComponentInChildren<CanvasGroup>();
		}

		public void FadeImmediate(float target)
		{
			canvasGroup.alpha = target;
		}

		public Coroutine FadeOut(float time)
		{
			return Fade(1, time);
		}

		public Coroutine FadeIn(float time)
		{
			return Fade(0, time);
		}

		private Coroutine Fade(float target, float time) 
		{
			if (activeCoroutine != null) StopCoroutine(activeCoroutine);
			activeCoroutine = StartCoroutine(FadeRoutine(target, time)); 
			return activeCoroutine;
		}

		private IEnumerator FadeRoutine(float target, float time)
		{
			if (Mathf.Approximately(canvasGroup.alpha, target))
				canvasGroup.alpha = Mathf.RoundToInt(1 * (1 - target));

			while (!Mathf.Approximately(canvasGroup.alpha, target))
			{
				//mathf.movetowards to get float to desired value using desired speed
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, 
					Time.deltaTime / time);
				yield return null;
			}
		}
	}
}
