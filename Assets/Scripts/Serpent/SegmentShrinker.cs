using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentShrinker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SerpentMovement mover;

		//States
		float targetDisToScreen;
		bool resizing = false;
		float sizeAtStart = 1f, sizeAtTarget = .25f;

		private void Update() 
		{
			if (resizing)
			{
				var screenTrans = Camera.main.WorldToViewportPoint(transform.position);
				screenTrans = new Vector3(screenTrans.x, screenTrans.y, 0);
				var worldScreenTrans = Camera.main.ViewportToWorldPoint(screenTrans);

				var disToScreen = Vector3.Distance(worldScreenTrans, transform.position);

				//formula is actually Mathf.Clamp01((disToScreen - distanceStart) / (distanceEnd - distanceStart)) 
				var disNormalized = Mathf.Clamp01(disToScreen / targetDisToScreen);
				var newSize = Mathf.Lerp(sizeAtStart, sizeAtTarget, disNormalized);
				
				transform.localScale = new Vector3(newSize, newSize, newSize);
				if (mover != null) mover.segmentSpacing = newSize;
			}
		}

		public void SetTargetData(Transform target, float startSize, float targetSize)
		{
			sizeAtStart = startSize; sizeAtTarget = targetSize;

			var targetScreenTrans = Camera.main.WorldToViewportPoint(target.position);
			targetScreenTrans = new Vector3(targetScreenTrans.x, targetScreenTrans.y, 0);
			var targetWorldScreenTrans = Camera.main.ViewportToWorldPoint(targetScreenTrans);
			
			targetDisToScreen = Vector3.Distance(targetWorldScreenTrans, target.position);

			resizing = true;
		}
	}
}
