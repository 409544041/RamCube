using Qbism.Serpent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class ScreenDistanceShrinker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SerpentMovement mover;

		//States
		float targetDisToScreen, startDisToScreen;
		public bool resizing { get; set; } = false;
		float sizeAtStart = 1f, sizeAtTarget = .25f;

		private void Update()
		{
			if (resizing)
			{
				var screenTrans = Camera.main.WorldToViewportPoint(transform.position);
				screenTrans = new Vector3(screenTrans.x, screenTrans.y, 0);
				var worldScreenTrans = Camera.main.ViewportToWorldPoint(screenTrans);

				var disToScreen = Vector3.Distance(worldScreenTrans, transform.position);

				//formula is actually Mathf.Clamp01((disToScreen - disATStart) / (targetDisToScreen - disAtStart)) 
				var disNormalized = Mathf.Clamp01((disToScreen - startDisToScreen) / 
					(targetDisToScreen - startDisToScreen));
				var newSize = Mathf.Lerp(sizeAtStart, sizeAtTarget, disNormalized);

				transform.localScale = new Vector3(newSize, newSize, newSize);
				if (mover != null) mover.segmentSpacing = newSize;
			}
		}

		public void SetTargetData(Vector3 targetPos, float startSize, float targetSize, float disAtStart)
		{
			sizeAtStart = startSize; sizeAtTarget = targetSize; startDisToScreen = disAtStart;

			var targetScreenTrans = Camera.main.WorldToViewportPoint(targetPos);
			var newTargetScreenTrans = new Vector3(targetScreenTrans.x, targetScreenTrans.y, 0);
			var targetWorldScreenTrans = Camera.main.ViewportToWorldPoint(newTargetScreenTrans);

			targetDisToScreen = Vector3.Distance(targetWorldScreenTrans, targetPos);

			resizing = true;
		}
	}
}
