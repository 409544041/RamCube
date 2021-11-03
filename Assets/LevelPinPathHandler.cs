using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinPathHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] LevelPin pin;
		public Transform pathPoint;
		public LineRenderer[] fullLineRenderers;
		public LineRenderer dottedLineRenderer;

		//Actions, events, delegates etc
		public Func<E_Pin, LevelPin> onGetPin;
		public event Action<Transform, List<LineDrawData>, LineRenderer[], LineRenderer> onPathCreation;

		//States
		List<LineDrawData> lineDestList = new List<LineDrawData>();
		int amountInDrawList = 0;

		public void CheckPathStatus(E_Pin uPin, bool uUnlocked, bool uUnlockAnimPlayed,
			int uLocksLeft, bool completed, int listCount)
		{
			AddToList(uPin, completed, uUnlocked, uUnlockAnimPlayed, uLocksLeft, lineDestList);
			amountInDrawList++;

			if (amountInDrawList == listCount)
				CreatePath(lineDestList);
		}

		private void AddToList(E_Pin uPin, bool completed, bool unlockStatus, bool unlockAnim,
			int locks, List<LineDrawData> lineDestList)
		{
			LevelPin destPin = onGetPin(uPin);

			int locksAmount = destPin.m_levelData.f_LocksAmount;
			bool lessLocks = locksAmount > locks && locks > 0;
			bool noLocks = locksAmount > locks && locks == 0;

			if ((completed && lessLocks) || (!pin.justCompleted && completed && noLocks && !unlockAnim))
			{
				LineDrawData drawData = new LineDrawData();
				drawData.destination = destPin.pinPather.pathPoint;
				drawData.lineType = LineTypes.dotted;
				lineDestList.Add(drawData);
			}

			if (completed && unlockStatus && unlockAnim)
			{
				LineDrawData drawData = new LineDrawData();
				drawData.destination = destPin.pinPather.pathPoint;
				drawData.lineType = LineTypes.full;
				lineDestList.Add(drawData);
			}
		}

		private void CreatePath(List<LineDrawData> lineDestList)
		{
			if (lineDestList.Count > 0)
			{
				for (int i = 0; i < lineDestList.Count; i++)
				{
					if (lineDestList[i].lineType == LineTypes.full)
					{
						LineDrawer fullDrawer = fullLineRenderers[i].GetComponent<LineDrawer>();
						fullDrawer.SetPositions(pathPoint, lineDestList[i].destination, false);
						fullLineRenderers[i].enabled = true;
					}
					else if (lineDestList[i].lineType == LineTypes.dotted)
					{
						LineDrawer dotDrawer = dottedLineRenderer.GetComponent<LineDrawer>();
						dotDrawer.SetPositions(pathPoint, lineDestList[i].destination, false);
						dottedLineRenderer.enabled = true;
					}
				}
			}
		}
	}
}
