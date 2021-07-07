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
		FinishEndSeqHandler finishEndSeq;

		private void Awake() 
		{
			canvasGroup = GetComponent<CanvasGroup>();	
			finishEndSeq = FindObjectOfType<FinishEndSeqHandler>();
		}

		private void OnEnable() 
		{
			if (finishEndSeq != null) finishEndSeq.onUIFade += StartFade;
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
			if (finishEndSeq != null) finishEndSeq.onUIFade -= StartFade;
		}
	}
}
