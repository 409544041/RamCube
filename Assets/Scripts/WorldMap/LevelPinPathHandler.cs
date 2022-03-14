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
		[SerializeField] LayerMask gateRayMask;
		[SerializeField] LevelPinRefHolder refs;

		//Actions, events, delegates etc
		public Func<E_Pin, LevelPinRefHolder> onGetPin;

		//States
		List<LineDrawData> lineDestList = new List<LineDrawData>();
		int amountInDrawList = 0;
		public bool justCompleted { get; set; } = false;

		public void CheckPathStatus(E_Pin uPin, bool uUnlocked, bool uUnlockAnimPlayed, int uLocksLeft,
			bool completed, int listCount, List<E_MapWalls> originWalls, bool wallDown, 
			bool dottedAnimPlayed, bool unlocked, bool unlockAnimPlayed)
		{
			AddToList(uPin, completed, uUnlocked, uUnlockAnimPlayed, uLocksLeft, 
				lineDestList, originWalls, wallDown, dottedAnimPlayed, unlocked, unlockAnimPlayed);
			amountInDrawList++;

			if (amountInDrawList == listCount)
				CreatePath(lineDestList);
		}

		private void AddToList(E_Pin uPin, bool completed, bool uUnlocked, bool uUnlockAnimPlayed, 
			int uLocksLeft, List<LineDrawData> lineDestList, List<E_MapWalls> uOriginWalls, 
			bool wallDown, bool dottedAnimPlayed, bool unlocked, bool unlockAnimPlayed)
		{
			LevelPinRefHolder destPin = onGetPin(uPin);
			var dest = destPin.pathPoint;
			bool linkedWall = false;

			if (uOriginWalls != null)
			{
				for (int i = 0; i < uOriginWalls.Count; i++)
				{
					if (refs.m_levelData.f_Pin == uOriginWalls[i].f_OriginPin) linkedWall = true;
				}
			}
			
			int uLocksAmount = destPin.m_levelData.f_LocksAmount;
			bool uLessLocks = uLocksAmount > uLocksLeft && uLocksLeft > 0;
			
			//if uPin still has a lock left but wall between pins is not down
			if (completed && uLessLocks && linkedWall && !wallDown && dottedAnimPlayed)
			{
				SetWallPathPoint(dest);
				FillAndAddDrawData(lineDestList, refs.wallPathPoint, LineTypes.dotted);
			}

			//same as below but now if there's a wall in between
			else if (!refs.pinPather.justCompleted && completed && uUnlocked &&
				!uUnlockAnimPlayed && linkedWall && dottedAnimPlayed)
			{
				SetWallPathPoint(dest);
				FillAndAddDrawData(lineDestList, refs.wallPathPoint, LineTypes.dotted);
			}

			//if uPin still has a lock left or if uPin has no locks left but on map load
			//the uPin unlock anim hasn't been played yet so dotted line is still needed
			else if ((completed && uLessLocks && dottedAnimPlayed) || 
				(!refs.pinPather.justCompleted && completed && 
				uUnlocked && !uUnlockAnimPlayed && dottedAnimPlayed))
				FillAndAddDrawData(lineDestList, dest, LineTypes.dotted);

			//if uPin is unlocked and unlock anim played etc drawing full line
			else if (!refs.pinPather.justCompleted && completed && uUnlocked && uUnlockAnimPlayed)
				FillAndAddDrawData(lineDestList, dest, LineTypes.full);

			//when debug-unlocking
			else if (!refs.pinPather.justCompleted && unlocked && unlockAnimPlayed && 
				uUnlocked && uUnlockAnimPlayed)
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
						refs.fullLineDrawers[i].SetPositions(refs.pathPoint, lineDestList[i].destination);
						refs.fullLineRenderers[i].enabled = true;
					}
					else if (lineDestList[i].lineType == LineTypes.dotted)
					{
						refs.dottedLineDrawer.SetPositions(refs.pathPoint, lineDestList[i].destination);
						refs.dottedLineRenderer.enabled = true;
					}
				}
			}
		}

		public void DrawToGate(LineTypes lineTypes, Transform destPoint)
		{
			SetWallPathPoint(destPoint);
			DrawNewPath(LineTypes.dotted, refs.wallPathPoint);
		}

		private void SetWallPathPoint(Transform destPoint)
		{
			RaycastHit wallHit;
			Vector3 rayDir = (destPoint.position - refs.pathPoint.position).normalized;

			if (Physics.Raycast(refs.pathPoint.position, rayDir, out wallHit, 10, gateRayMask))
			{
				var gateDist = Vector3.Distance(refs.pathPoint.position, wallHit.point) - .25f;
				refs.wallPathPoint.position = refs.pathPoint.position + (rayDir * gateDist);
			}
		}

		public void DrawNewPath(LineTypes lineType, Transform destPoint)
		{
			List<LineRenderer> lineRenders = new List<LineRenderer>();
			if (lineType == LineTypes.full)
			{
				for (int i = 0; i < refs.fullLineRenderers.Length; i++)
				{
					lineRenders.Add(refs.fullLineRenderers[i]);
				}
			} 
			else if (lineType == LineTypes.dotted) lineRenders.Add(refs.dottedLineRenderer);

			for (int i = 0; i < lineRenders.Count; i++)
			{
				var render = lineRenders[i];
				var drawer = render.GetComponent<LineDrawer>();

				//if first path is already drawing or drawn skip to second renderer
				if (drawer.drawing || render.enabled) continue; 
				PrepareDrawer(destPoint, render, drawer);
				break;
			}
		}

		private void PrepareDrawer(Transform destPoint, LineRenderer render, LineDrawer drawer)
		{
			drawer.pointToMove = 1;
			drawer.SetPositions(refs.pathPoint, destPoint);
			drawer.InitiateLineDrawing();
			render.enabled = true;
		}
	}
}
