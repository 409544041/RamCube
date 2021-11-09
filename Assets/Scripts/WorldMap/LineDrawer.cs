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
		[SerializeField] float drawStep = 7f, lineWidth = .15f, linePinBuffer = 1;


		//Cache
		LineRenderer lRender;

		//States
		public bool drawing { get; set; } = false;
		float counter = 0;
		Vector3 originPos, destPos;
		LevelPin destLevelPin;
		Vector3 pointAlongLine;
		public int pointToMove { get; set; } = 0;
		LevelPin originPin = null;

		//Actions, events, delegates etc
		public event Action onSaveData;
		public event Action<E_LevelGameplayData, bool> onDisableLockInSheet;

		private void Awake()
		{
			lRender = GetComponent<LineRenderer>();
			SetLineWidth(0);
		}

		public void SetPositions(Transform incOrigin, Transform incDest)
		{
			destLevelPin = incDest.GetComponentInParent<LevelPin>();
			originPin = incOrigin.GetComponentInParent<LevelPin>();
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

		public Vector3 SetOriginPoint(Vector3 origin, Vector3 dest, float x)
		{
			var point = x * Vector3.Normalize(dest - origin) + origin;
			return point;
		}

		public Vector3 SetDestPoint(Vector3 origin, Vector3 dest, float x)
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

			if (lRender.startWidth == 0 && lRender.endWidth == 0)
				SetLineWidth(lineWidth);

			var drawSpeed = drawStep * Time.deltaTime;
			pointAlongLine = originPos;

			while(Vector3.Distance(pointAlongLine, destPos) > .1f)
			{
				pointAlongLine = Vector3.MoveTowards(pointAlongLine, destPos, drawSpeed);
				lRender.SetPosition(pointToMove, pointAlongLine);

				yield return null;
			}

			drawing = false;

			var ent = E_LevelGameplayData.FindEntity(entity =>
					entity.f_Pin == destLevelPin.m_levelData.f_Pin);

			if (ent.f_LocksLeft == 0)
			{
				onDisableLockInSheet(ent, true);
				var pinUI = destLevelPin.pinUI;
				pinUI.DisableLockIcon();

				//if locks = 0 but there were more locks means full lines are drawn.
				//Remove any dotted lines after full lines are drawn
				if (destLevelPin.m_levelData.f_LocksAmount > 0) 
					originPin.pinPather.dottedLineRenderer.GetComponent<LineRenderer>().enabled = false;
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
