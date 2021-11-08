using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinPathHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] LevelPin pin;
		public Transform pathPoint, wallPathPoint;
		public LineRenderer[] fullLineRenderers;
		public LineRenderer dottedLineRenderer;
		[SerializeField] LayerMask gateRayMask;

		//Actions, events, delegates etc
		public Func<E_Pin, LevelPin> onGetPin;
		public event Action<Transform, List<LineDrawData>, LineRenderer[], LineRenderer> onPathCreation;

		//States
		List<LineDrawData> lineDestList = new List<LineDrawData>();
		int amountInDrawList = 0;

		public void CheckPathStatus(E_Pin uPin, bool uUnlocked, bool uUnlockAnimPlayed, int uLocksLeft,
			bool completed, int listCount, List<E_MapWalls> originWalls, bool wallDown, bool dottedAnimPlayed)
		{
			AddToList(uPin, completed, uUnlocked, uUnlockAnimPlayed, uLocksLeft, 
				lineDestList, originWalls, wallDown, dottedAnimPlayed);
			amountInDrawList++;

			if (amountInDrawList == listCount)
				CreatePath(lineDestList);
		}

		private void AddToList(E_Pin uPin, bool completed, bool uUnlocked, bool uUnlockAnimPlayed, int uLocksLeft,
			List<LineDrawData> lineDestList, List<E_MapWalls> uOriginWalls, bool wallDown, bool dottedAnimPlayed)
		{
			LevelPin destPin = onGetPin(uPin);
			var dest = destPin.pinPather.pathPoint;
			bool linkedWall = false;

			if (uOriginWalls != null)
			{
				for (int i = 0; i < uOriginWalls.Count; i++)
				{
					if (pin.m_levelData.f_Pin == uOriginWalls[i].f_OriginPin) linkedWall = true;
				}
			}
			
			int uLocksAmount = destPin.m_levelData.f_LocksAmount;
			bool uLessLocks = uLocksAmount > uLocksLeft && uLocksLeft > 0;
			
			//if uPin still has a lock left but wall between pins is not down
			if (completed && uLessLocks && linkedWall && !wallDown && dottedAnimPlayed)
			{
				SetWallPathPoint(dest);
				FillAndAddDrawData(lineDestList, wallPathPoint, LineTypes.dotted);
			}

			//same as below but now if there's a wall in between
			else if (!pin.justCompleted && completed && uUnlocked &&
				!uUnlockAnimPlayed && linkedWall && dottedAnimPlayed)
			{
				SetWallPathPoint(dest);
				FillAndAddDrawData(lineDestList, wallPathPoint, LineTypes.dotted);
			}

			//if uPin still has a lock left or if uPin has no locks left but on map load
			//the uPin unlock anim hasn't been played yet so dotted line is still needed
			else if ((completed && uLessLocks && dottedAnimPlayed) || (!pin.justCompleted && completed && 
				uUnlocked && !uUnlockAnimPlayed && dottedAnimPlayed))
				FillAndAddDrawData(lineDestList, dest, LineTypes.dotted);

			//if uPin is unlocked and unlock anim played etc drawing full line
			else if (!pin.justCompleted && completed && uUnlocked && uUnlockAnimPlayed)
				FillAndAddDrawData(lineDestList, dest, LineTypes.full);
		}

		private void FillAndAddDrawData(List<LineDrawData> lineDestList, Transform dest,
			LineTypes lineType)
		{
			LineDrawData drawData = new LineDrawData();
			drawData.destination = dest;
			drawData.lineType = lineType;
			lineDestList.Add(drawData);
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
						fullDrawer.SetPositions(pathPoint, lineDestList[i].destination);
						fullLineRenderers[i].enabled = true;
					}
					else if (lineDestList[i].lineType == LineTypes.dotted)
					{
						LineDrawer dotDrawer = dottedLineRenderer.GetComponent<LineDrawer>();
						dotDrawer.SetPositions(pathPoint, lineDestList[i].destination);
						dottedLineRenderer.enabled = true;
					}
				}
			}
		}

		public void DrawToGate(LineTypes lineTypes, Transform destPoint)
		{
			SetWallPathPoint(destPoint);

			DrawNewPath(LineTypes.dotted, wallPathPoint);
		}

		private void SetWallPathPoint(Transform destPoint)
		{
			RaycastHit wallHit;
			Vector3 rayDir = (destPoint.position - pathPoint.position).normalized;

			if (Physics.Raycast(pathPoint.position, rayDir, out wallHit, 10, gateRayMask))
			{
				var gateDist = Vector3.Distance(pathPoint.position, wallHit.point) - .25f;
				wallPathPoint.position = pathPoint.position + (rayDir * gateDist);
			}
		}

		public void DrawNewPath(LineTypes lineType, Transform destPoint)
		{
			List<LineRenderer> lineRenders = new List<LineRenderer>();
			if (lineType == LineTypes.full)
			{
				for (int i = 0; i < fullLineRenderers.Length; i++)
				{
					lineRenders.Add(fullLineRenderers[i]);
				}
			} 
			else if (lineType == LineTypes.dotted) lineRenders.Add(dottedLineRenderer);

			for (int i = 0; i < lineRenders.Count; i++)
			{
				LineRenderer render = lineRenders[i];
				LineDrawer drawer = render.GetComponent<LineDrawer>();

				//if first path is already drawing or drawn skip to second renderer
				if (drawer.drawing || render.enabled) continue; 
				PrepareDrawer(destPoint, render, drawer);
				break;
			}
		}

		private void PrepareDrawer(Transform destPoint, LineRenderer render, LineDrawer drawer)
		{
			drawer.pointToMove = 1;
			drawer.SetPositions(pin.pinPather.pathPoint, destPoint);
			drawer.InitiateLineDrawing();
			render.enabled = true;
		}
	}
}
