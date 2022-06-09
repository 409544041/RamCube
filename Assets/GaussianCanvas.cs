using Qbism.Serpent;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class GaussianCanvas : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Canvas canvas;
		[SerializeField] CanvasGroup group;
		[SerializeField] Camera gaussianCam;
		[SerializeField] GameplayCoreRefHolder gcRef;
		[SerializeField] SerpCoreRefHolder scRef;
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		Camera cam;

		private void Awake()
		{
			if (gcRef != null) cam = gcRef.cam;
			else if (scRef != null) cam = scRef.cam;
			else if (mcRef != null) cam = mcRef.cam;
		}

		public void SetUpGaussianCanvas()
		{
			if (group.alpha == 1) return;

			if (scRef != null) scRef.bgSerpCanvas.worldCamera = gaussianCam;
			gaussianCam.orthographicSize = cam.orthographicSize;
			canvas.transform.parent = cam.transform;
			canvas.transform.rotation = cam.transform.rotation;
			canvas.transform.localPosition = new Vector3(0, 0, 10);
			group.alpha = 1;
		}

		public void TurnOffGaussianCanvas()
		{
			if (scRef != null) scRef.bgSerpCanvas.worldCamera = cam;
			group.alpha = 0;
		}
	}
}
