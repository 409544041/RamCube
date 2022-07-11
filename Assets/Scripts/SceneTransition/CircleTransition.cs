using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Qbism.Saving;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.SceneTransition
{
	public class CircleTransition : MonoBehaviour
	{
		//Config parameters
		public Image circle;
		[SerializeField] float circleStartSize = 2500;
		[SerializeField] float transDur = 1;
		[SerializeField] PersistentRefHolder persRef;

		//Cache
		Coroutine activeCoroutine = null;

		public Coroutine TransIn()
		{
			return Transition(1);
		}

		public Coroutine TransOut()
		{
			return Transition(0);
		}

		public void SetCirclePos(Vector3 worldPos)
		{
			var screenpoint = persRef.cam.WorldToScreenPoint(worldPos);
			Debug.Log("Screenpoint = " + screenpoint + " & worldPos = " + worldPos);
			Debug.Break();
			circle.rectTransform.anchoredPosition =
				new Vector3(screenpoint.x /*- persRef.circCanvas.transform.position.x*/,
				screenpoint.y /*- persRef.circCanvas.transform.position.y*/, 0);
		}

		private Coroutine Transition(int target)
		{
			if (activeCoroutine != null) StopCoroutine(activeCoroutine);
			activeCoroutine = StartCoroutine(AnimateTransition(target));
			return activeCoroutine;
		}

		private IEnumerator AnimateTransition(int target)
		{
			var startSize = circle.rectTransform.sizeDelta;
			var scaleTarget = circleStartSize * target;
			var endSize = new Vector2(scaleTarget, scaleTarget);
			float elapsedTime = 0;

			while (!Mathf.Approximately(circle.rectTransform.sizeDelta.x, scaleTarget))
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / transDur;

				circle.rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, percentageComplete);

				yield return null;
			}
		}

		public void SetCircleStartState(int target)
		{
			var scaleTarget = circleStartSize * target;

			if (Mathf.Approximately(circle.rectTransform.sizeDelta.x, scaleTarget))
			{
				var startSize = Mathf.RoundToInt(circleStartSize * (1 - target));
				circle.rectTransform.sizeDelta = new Vector2(startSize, startSize);
			}
		}

		public void ForceCircleSize(int target)
		{
			var size = circleStartSize * target;
			circle.rectTransform.sizeDelta = new Vector2(size, size);
		}

		public void DebugFixCircleMask()
		{
			//needed bc otherwise circle won't show at all
			//due to bug in code (from Codemonkey tutorial)
			circle.RecalculateMasking();
			circle.enabled = false;
			circle.enabled = true;
		}
	}
}
