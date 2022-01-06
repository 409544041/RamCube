using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class CamResizer : MonoBehaviour
	{
		public void InitiateCamResize(CinemachineVirtualCamera cam, float targetSize, float resizeDur,
			AnimationCurve curve)
		{
			StartCoroutine(ResizeCam(cam, targetSize, resizeDur, curve));
		}

		private IEnumerator ResizeCam(CinemachineVirtualCamera cam, float targetSize, float resizeDur,
			AnimationCurve curve)
		{
			var startSize = cam.m_Lens.OrthographicSize;

			for (float t = 0; t < resizeDur; t += Time.deltaTime)
			{
				var percentageCompleted = t / resizeDur;
				float size;

				if (curve != null)
					size = Mathf.Lerp(startSize, targetSize, curve.Evaluate(percentageCompleted));

				else size = Mathf.Lerp(startSize, targetSize, percentageCompleted);

				cam.m_Lens.OrthographicSize = size;

				yield return null;
			}
		}

		public void InitiateCamDollyMove(CinemachineVirtualCamera cam, float target, float travelDur,
			AnimationCurve curve)
		{
			StartCoroutine(CamDollyMove(cam, target, travelDur, curve));
		}

		private IEnumerator CamDollyMove(CinemachineVirtualCamera cam, float target, float travelDur,
			AnimationCurve curve)
		{
			var dollyComp = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
			var startPos = dollyComp.m_PathPosition;

			for (float t = 0; t < travelDur; t += Time.deltaTime)
			{
				var percentageCompleted = t / travelDur;

				var pos = Mathf.Lerp(startPos, target, curve.Evaluate(percentageCompleted));

				dollyComp.m_PathPosition = pos;

				yield return null;
			}
		}
	}
}