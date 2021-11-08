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
		[SerializeField] float drawStep = 7f;
		[SerializeField] float lineWidth = .15f;

		//Cache
		LineRenderer lRender;

		//States
		public bool drawing { get; set; } = false;
		float counter = 0;
		float distance = 0;
		public Transform origin { get; set; }
		public Transform destination { get; set; }
		LevelPin destLevelPin;
		Vector3 pointAlongLine;
		public int pointToMove = 0;
		LevelPin originPin = null;

		//Actions, events, delegates etc
		public event Action onSaveData;
		public event Action<E_LevelGameplayData, bool> onDisableLockInSheet;

		private void Awake()
		{
			lRender = GetComponent<LineRenderer>();
			SetLineWidth(0);
		}

		public void SetPositions(Transform incOrigin, Transform incDestination)
		{
			origin = incOrigin;
			destination = incDestination;
			destLevelPin = destination.GetComponentInParent<LevelPin>();
			lRender.positionCount = 2;
			lRender.SetPosition(0, origin.position);
			originPin = incOrigin.GetComponentInParent<LevelPin>();

			if (drawing)
				distance = Vector3.Distance(origin.position, destination.position);

			if (!drawing)
			{
				lRender.SetPosition(1, destination.position);
				SetLineWidth(lineWidth);
			}
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
			pointAlongLine = origin.position;

			while(Vector3.Distance(pointAlongLine, destination.position) > .1f)
			{
				pointAlongLine = Vector3.MoveTowards(pointAlongLine, destination.position, drawSpeed);
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
