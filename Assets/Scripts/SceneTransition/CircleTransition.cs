using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Qbism.WorldMap;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
	//Config parameters
	public Image circle;
	[SerializeField] float circleStartSize = 2500;
	[SerializeField] float shrinkSpeed = 3000f;

	//Cache
	Canvas canvas = null;
	Coroutine activeCoroutine = null;

	private void Awake() 
	{
		canvas = GetComponentInChildren<Canvas>();
	}

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
		var screenpoint = Camera.main.WorldToScreenPoint(worldPos);

		circle.rectTransform.anchoredPosition =
			new Vector3(screenpoint.x - canvas.transform.position.x, 
			screenpoint.y - canvas.transform.position.y, 0);
	}

	private Coroutine Transition(int target)
	{
		if (activeCoroutine != null) StopCoroutine(activeCoroutine);
		activeCoroutine = StartCoroutine(AnimateTransition(target));
		return activeCoroutine;
	}

	private IEnumerator AnimateTransition(int target)
	{
		var scaleTarget = circleStartSize * target;

		while (!Mathf.Approximately(circle.rectTransform.sizeDelta.x, scaleTarget))
		{
			var size = Mathf.MoveTowards(circle.rectTransform.sizeDelta.x, scaleTarget,
				shrinkSpeed * Time.deltaTime);

			circle.rectTransform.sizeDelta = new Vector2(size, size);

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
