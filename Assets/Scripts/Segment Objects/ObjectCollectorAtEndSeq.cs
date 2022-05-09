using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Qbism.Objects
{
	public class ObjectCollectorAtEndSeq : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float overlayDelay = 4, objectScaleUp = 3, scaleTransDur = .5f;
		[SerializeField] AnimationCurve scaleCurve;
		[SerializeField] GameplayCoreRefHolder gcRef;

		//Cache
		public GameObject collectableObject { get; set; }
		public SegmentObjectJuicer objJuicer { get; set; }
		Vector3 objNewViewPos;
		public Transform objMeshTrans { get; set; }
		public bool overlayActive { get; private set; } = false;
		public M_Objects m_Object { get; set; }

		public void InitiateShowingObjectOverlay()
		{
			overlayActive = true;

			var screenStateMngr = gcRef.glRef.screenStateMngr;
			screenStateMngr.SwitchState(screenStateMngr.objectOverlayState);

			StartCoroutine(ShowObjectOverlay());
		}

		private IEnumerator ShowObjectOverlay()
		{
			yield return new WaitForSeconds(overlayDelay);

			GetObjCloserToCam();
			SetupBackgroundCanvas();
			SetupVFX();
			StartCoroutine(ScaleObject());

			gcRef.objectNameText.text = m_Object.f_ObjectTitle;
			gcRef.objectSubText.text = m_Object.f_ObjectSubText;
			gcRef.objectOwnerText.text = "Belongs to " + m_Object.f_Owner.f_SegmentName;

			gcRef.objectCanvasGroup.alpha = 1;
		}

		private void GetObjCloserToCam()
		{
			var viewPos = gcRef.cam.WorldToViewportPoint(collectableObject.transform.position);
			objNewViewPos = new Vector3(viewPos.x, viewPos.y, 5);
			collectableObject.transform.position = gcRef.cam.ViewportToWorldPoint(objNewViewPos);
			collectableObject.transform.parent = gcRef.cam.transform;
		}

		private void SetupBackgroundCanvas()
		{
			gcRef.bgCanvas.transform.parent = gcRef.cam.transform;
			gcRef.bgCanvas.transform.rotation = gcRef.cam.transform.rotation;
			gcRef.bgCanvas.transform.localPosition = new Vector3(0, 0, 10);
			gcRef.bgCanvasGroup.alpha = 1;
			gcRef.bgCanvas.sortingOrder = 0;
		}

		private void SetupVFX()
		{
			objJuicer.uiStar.transform.forward = gcRef.cam.transform.forward;
			objJuicer.fartParticles.transform.forward = gcRef.cam.transform.forward;

			var objMeshViewPos = gcRef.cam.WorldToViewportPoint(objMeshTrans.position);
			var starNewViewPos = new Vector3(objMeshViewPos.x, objMeshViewPos.y, 8);
			var particleNewVewPos = new Vector3(objMeshViewPos.x, objMeshViewPos.y, 7);

			objJuicer.uiStar.transform.position = gcRef.cam.ViewportToWorldPoint(starNewViewPos);
			objJuicer.fartParticles.transform.position = gcRef.cam.ViewportToWorldPoint(particleNewVewPos);
		}
		
		private IEnumerator ScaleObject()
		{
			var startscale = objMeshTrans.localScale;
			var newScale = startscale.x * objectScaleUp;
			var targetScale = new Vector3(newScale, newScale, newScale);
			float elapsedTime = 0;

			while (!Mathf.Approximately(objMeshTrans.localScale.x, newScale))
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / scaleTransDur;

				objMeshTrans.localScale = Vector3.Lerp(startscale, targetScale,
					scaleCurve.Evaluate(percentageComplete));

				yield return null;
			}

			objJuicer.TriggerScaleUpJuice();
			objJuicer.TriggerVFX();
		}
	}
}
