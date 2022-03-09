using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class ObjectCollectManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Canvas backgroundCanvas;
		[SerializeField] float overlayDelay = 4, objectScaleUp = 3, scaleTransDur = .5f;
		[SerializeField] AnimationCurve scaleCurve;
		[SerializeField] Camera cam;

		//Cache
		GameObject collectableObject;
		public SegmentObjectJuicer objJuicer { get; set; }

		public void InitiateShowingObjectOverlay()
		{
			StartCoroutine(ShowObjectOverlay());
		}

		private IEnumerator ShowObjectOverlay()
		{
			yield return new WaitForSeconds(overlayDelay);

			GetObjCloserToCam();
			SetupBackgroundCanvas();
			StartCoroutine(ScaleObject());
		}

		private void GetObjCloserToCam()
		{
			collectableObject = GameObject.FindGameObjectWithTag("SegmentObject");
			var viewPortPos = cam.WorldToViewportPoint(collectableObject.transform.position);
			var newViewPortPos = new Vector3(viewPortPos.x, viewPortPos.y, 5);
			collectableObject.transform.position = cam.ViewportToWorldPoint(newViewPortPos);
			collectableObject.transform.parent = cam.transform;
		}

		private void SetupBackgroundCanvas()
		{
			backgroundCanvas.transform.parent = cam.transform;
			backgroundCanvas.transform.rotation = cam.transform.rotation;
			backgroundCanvas.transform.localPosition = new Vector3(0, 0, 10);
			backgroundCanvas.GetComponent<CanvasGroup>().alpha = 1;
		}
		
		private IEnumerator ScaleObject()
		{
			var objMesh = collectableObject.GetComponentInChildren<Renderer>();
			var startscale = objMesh.transform.localScale;
			var newScale = startscale.x * objectScaleUp;
			var targetScale = new Vector3(newScale, newScale, newScale);
			float elapsedTime = 0;

			while (!Mathf.Approximately(objMesh.transform.localScale.x, newScale))
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / scaleTransDur;

				objMesh.transform.localScale = Vector3.Lerp(startscale, targetScale,
					scaleCurve.Evaluate(percentageComplete));

				yield return null;
			}

			objJuicer.TriggerScaleUpJuice();
		}
	}
}
