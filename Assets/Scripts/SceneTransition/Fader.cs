using Qbism.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.SceneTransition
{
	public class Fader : MonoBehaviour
	{
		//Config parameters
		public float sceneTransTime;
		[SerializeField] PersistentRefHolder persRef;

		//Cache
		Coroutine activeCoroutine = null;

		public void FadeImmediate(float target)
		{
			persRef.fadeCanvasGroup.alpha = target;
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
			if (Mathf.Approximately(persRef.fadeCanvasGroup.alpha, target))
				persRef.fadeCanvasGroup.alpha = Mathf.RoundToInt(1 * (1 - target));

			while (!Mathf.Approximately(persRef.fadeCanvasGroup.alpha, target))
			{
				//mathf.movetowards to get float to desired value using desired speed
				persRef.fadeCanvasGroup.alpha = Mathf.MoveTowards(persRef.fadeCanvasGroup.alpha, target, 
					Time.deltaTime / time);
				yield return null;
			}
		}
	}
}
