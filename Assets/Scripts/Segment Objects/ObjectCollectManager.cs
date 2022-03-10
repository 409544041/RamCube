using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Qbism.Objects
{
	public class ObjectCollectManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Canvas objectCanvas, backgroundCanvas;
		[SerializeField] TextMeshProUGUI objNameText, objSubText, objOwnerText;
		[SerializeField] float overlayDelay = 4, objectScaleUp = 3, scaleTransDur = .5f;
		[SerializeField] AnimationCurve scaleCurve;
		[SerializeField] Camera cam;

		//Cache
		GameObject collectableObject;
		public SegmentObjectJuicer objJuicer { get; set; }
		Vector3 objNewViewPos;
		Renderer objMesh;

		public void InitiateShowingObjectOverlay()
		{
			StartCoroutine(ShowObjectOverlay());
		}

		private IEnumerator ShowObjectOverlay()
		{
			yield return new WaitForSeconds(overlayDelay);
			collectableObject = GameObject.FindGameObjectWithTag("SegmentObject");
			objMesh = collectableObject.GetComponentInChildren<Renderer>();
			var m_Object = collectableObject.GetComponent<M_Objects>();

			GetObjCloserToCam();
			SetupBackgroundCanvas();
			SetupStar();
			StartCoroutine(ScaleObject());

			objNameText.text = m_Object.f_ObjectTitle;
			objSubText.text = m_Object.f_ObjectSubText;
			objOwnerText.text = "Belongs to " + m_Object.f_Owner.f_SegmentName;

			objectCanvas.GetComponent<CanvasGroup>().alpha = 1;
		}

		private void GetObjCloserToCam()
		{
			var viewPos = cam.WorldToViewportPoint(collectableObject.transform.position);
			objNewViewPos = new Vector3(viewPos.x, viewPos.y, 5);
			collectableObject.transform.position = cam.ViewportToWorldPoint(objNewViewPos);
			collectableObject.transform.parent = cam.transform;
		}

		private void SetupBackgroundCanvas()
		{
			backgroundCanvas.transform.parent = cam.transform;
			backgroundCanvas.transform.rotation = cam.transform.rotation;
			backgroundCanvas.transform.localPosition = new Vector3(0, 0, 10);
			backgroundCanvas.GetComponent<CanvasGroup>().alpha = 1;
		}

		private void SetupStar()
		{
			objJuicer.uiStar.transform.forward = cam.transform.forward;
			var objMeshViewPos = cam.WorldToViewportPoint(objMesh.transform.position);
			var starNewViewPos = new Vector3(objMeshViewPos.x, objMeshViewPos.y, 7);
			objJuicer.uiStar.transform.position = cam.ViewportToWorldPoint(starNewViewPos);
		}
		
		private IEnumerator ScaleObject()
		{
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
			objJuicer.TriggerStarScaleJuice();
		}
	}
}
