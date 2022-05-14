using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentUIHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SegmentRefHolder refs;
		[SerializeField] FaceCamera camFacer;

		//States
		float padding;
		Vector3 leftScreenEdgePaddingWorldPos, rightScreenEdgePaddingWorldPos;

		private void Start()
		{
			if (refs.scRef == null)
			{
				refs.canvasGroup.alpha = 0;
				return;
			}
		}

		private void Update()
		{
			if (refs.scRef == null) return;
			UpdateMarkerPos();
			UpdateArrowDir();
			camFacer.FaceToCam();
		}

		private void UpdateMarkerPos()
		{
			var markerLeftPaddingPos = new Vector3(refs.markerTrans.position.x - padding,
				refs.markerTrans.position.y, refs.markerTrans.position.z);
			var markerRightPaddingPos = new Vector3(refs.markerTrans.position.x + padding,
				refs.markerTrans.position.y, refs.markerTrans.position.z);

			var markerLeftViewPortPos = refs.cam.WorldToViewportPoint(markerLeftPaddingPos);
			var markerRightViewPortPos = refs.cam.WorldToViewportPoint(markerRightPaddingPos);


			if (markerLeftViewPortPos.x > 0 && markerRightViewPortPos.x < 1)
			{
				if (refs.uiObject.transform.parent != refs.transform)
					refs.uiObject.transform.parent = refs.transform;

				if (refs.uiObject.transform.position != refs.markerTrans.position)
					refs.uiObject.transform.position = refs.markerTrans.position;
			}
			else if (markerLeftViewPortPos.x < 0)
			{
				if (refs.uiObject.transform.parent != null)
					refs.uiObject.transform.parent = null;

				if (refs.uiObject.transform.position != leftScreenEdgePaddingWorldPos)
					refs.uiObject.transform.position = leftScreenEdgePaddingWorldPos;

			}
			else if (markerRightViewPortPos.x > 1)
			{
				if (refs.uiObject.transform.parent != null)
					refs.uiObject.transform.parent = null;

				if (refs.uiObject.transform.position != rightScreenEdgePaddingWorldPos)
					refs.uiObject.transform.position = rightScreenEdgePaddingWorldPos;
			}
		}

		private void UpdateArrowDir()
		{
			var dir = (refs.transform.position - refs.uiObject.transform.position).normalized;
			refs.notificationArrow.up = -dir;
		}
		public void SetScreenEdgePosWithPadding()
		{
			padding = refs.canvas.GetComponent<RectTransform>().rect.width / 2;

			var markerViewPortPos = refs.cam.WorldToViewportPoint(refs.markerTrans.position);
			var leftScreenEdgeViewPortPos = new Vector3(0, markerViewPortPos.y, markerViewPortPos.z);
			var rightScreenEdgeViewPortPos = new Vector3(1, markerViewPortPos.y, markerViewPortPos.z);

			var leftScreenEdgeWorldPos = refs.cam.ViewportToWorldPoint(leftScreenEdgeViewPortPos);
			var rightScreenEdgeWorldPos = refs.cam.ViewportToWorldPoint(rightScreenEdgeViewPortPos);

			leftScreenEdgePaddingWorldPos = new Vector3(leftScreenEdgeWorldPos.x + padding,
				leftScreenEdgeWorldPos.y, leftScreenEdgeWorldPos.z);
			rightScreenEdgePaddingWorldPos = new Vector3(rightScreenEdgeWorldPos.x - padding,
				rightScreenEdgeWorldPos.y, rightScreenEdgeWorldPos.z);
		}

		public void ToggleUIDependingOnObjectStatus()
		{
			bool showMarker = false;

			var objs = refs.mSegments.f_Objects;
			if (objs == null || !refs.mSegments.f_GameplayData.f_Rescued || refs.scRef == null)
			{
				refs.canvasGroup.alpha = 0;
				return;
			}

			for (int i = 0; i < objs.Count; i++)
			{
				var objData = objs[i].f_GameplayData;

				if (showMarker) continue;
				if (objData.f_ObjectFound && !objData.f_ObjectReturned) showMarker = true;
			}

			if (showMarker) refs.canvasGroup.alpha = 1;
			else refs.canvasGroup.alpha = 0;
		}

		public void SetCam()
		{
			camFacer.cam = refs.cam;
		}
	}

}