using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.WorldMap;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LineDrawer : MonoBehaviour
	{
		//Config parameters
		public LineTypes lineType = LineTypes.full;
		[SerializeField] float drawDur = .5f, lineWidth = .15f, linePinBuffer = 1;
		[SerializeField] ParticleSystem particle = null;
		[SerializeField] AnimationCurve drawCurve;
		[SerializeField] LineRenderer lRender;

		//States
		public bool drawing { get; set; } = false;
		Vector3 originPos, destPos;
		LevelPinRefHolder destLevelPin;
		Vector3 pointAlongLine;
		public int pointToMove { get; set; } = 0;
		LevelPinRefHolder originPin = null;

		//Actions, events, delegates etc
		public event Action onSaveData;
		public event Action<E_LevelGameplayData, bool> onDisableLockInSheet;

		private void Awake()
		{
			SetLineWidth(0);
		}

		public void SetPositions(Transform incOrigin, Transform incDest)
		{
			destLevelPin = incDest.GetComponentInParent<LevelPinRefHolder>();
			originPin = incOrigin.GetComponentInParent<LevelPinRefHolder>();
			originPos = SetOriginPoint(incOrigin.position, incDest.position, linePinBuffer);
			destPos = SetDestPoint(originPos, incDest.position, linePinBuffer);
			lRender.positionCount = 2;
			lRender.SetPosition(0, originPos);

			if (!drawing)
			{
				lRender.SetPosition(1, destPos);
				SetLineWidth(lineWidth);
			}
		}

		private Vector3 SetOriginPoint(Vector3 origin, Vector3 dest, float x)
		{
			var point = x * Vector3.Normalize(dest - origin) + origin;
			return point;
		}

		private Vector3 SetDestPoint(Vector3 origin, Vector3 dest, float x)
		{
			var lineDir = (dest - origin).normalized;
			var dist = Vector3.Distance(origin, dest) - x;
			var point = origin + (lineDir * dist);
			return point;
		}

		public void InitiateLineDrawing()
		{
			StartCoroutine(AnimateLineDrawing());
		}

		private IEnumerator AnimateLineDrawing()
		{
			drawing = true;
			particle.Play();

			if (lRender.startWidth == 0 && lRender.endWidth == 0)
				SetLineWidth(lineWidth);

			particle.transform.position = originPos;
			particle.transform.LookAt(destPos, Vector3.up);

			float elapstedTime = 0;

			while(Vector3.Distance(pointAlongLine, destPos) > .1f)
			{
				elapstedTime += Time.deltaTime;
				var percentComplete = elapstedTime / drawDur;

				pointAlongLine = Vector3.Lerp(originPos, destPos, drawCurve.Evaluate(percentComplete));
				lRender.SetPosition(pointToMove, pointAlongLine);

				particle.transform.position = pointAlongLine;

				yield return null;
			}

			drawing = false;
			particle.Stop();

			var ent = E_LevelGameplayData.FindEntity(entity =>
					entity.f_Pin == destLevelPin.m_levelData.f_Pin);

			if (ent.f_LocksLeft == 0)
			{
				onDisableLockInSheet(ent, true);
				var pinUI = destLevelPin.pinUI;

				//if locks = 0 but there were more locks means full lines are drawn.
				//Remove any dotted lines after full lines are drawn
				if (destLevelPin.m_levelData.f_LocksAmount > 0) 
					originPin.dottedLineRenderer.enabled = false;
			} 
			onSaveData();
		}

		private void SetLineWidth(float width)
		{
			lRender.startWidth = width;
			lRender.endWidth = width;
		}
	}
}
