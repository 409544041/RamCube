using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeUIHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool cubeUIActive = false;
		[SerializeField] LayerMask lineLayers;
		[SerializeField] float triggerDis = 0.3f;
		[SerializeField] LayerMask lineRayLayers;
		[SerializeField] Transform uiRayOrigin;
		[SerializeField] float uiRayDist = 3;
		[SerializeField] float elevatedLinePadding = 40;
		[SerializeField] CubeRefHolder refs;

		//Cache
		PlayerRefHolder pRef;
		PlayerCubeMover mover;
		FinishCube finishCube;
		MoveableCubeHandler movCubeHandler;

		//States
		float disToPlayer = 0;
		bool hiddenForFinish = false;
		public bool showCubeUI { get; set; } = false;
		public bool debugShowCubeUI { get; set; } = true;
		Vector3 lineTopPos;

		private void Awake() 
		{
			if (!cubeUIActive) return;
			pRef = refs.gcRef.pRef;
			mover = pRef.playerMover;
			finishCube = refs.gcRef.finishRef.finishCube;
			movCubeHandler = refs.gcRef.glRef.movCubeHandler;
			var uiPos = refs.uiElement.transform.localPosition;
			lineTopPos = new Vector3(uiPos.x, uiPos.y, uiPos.z + 10);
		}

		private void Start()
		{
			if (!cubeUIActive) return;
			CheckForMirror();
			refs.uiLineRender.SetPosition(1, lineTopPos);
		}

		private void Update()
		{
			if (!cubeUIActive) return;
			if (showCubeUI && debugShowCubeUI) ShowUICheck();
			if (finishCube.FetchFinishStatus() && !hiddenForFinish)	
				HideUIForFinish();
		}

		private void ShowUICheck()
		{
			disToPlayer = Vector3.Distance
				(refs.floorCube.transform.position, mover.transform.position);

			AdjustLineRenderLength();

			//The last part is to avoid bug when static cube becomes floor cube
			if (mover.isMoving || disToPlayer < triggerDis || refs.floorCube.type == CubeTypes.Shrinking)
			{
				ShowOrHideUI(false);
				return;
			}

			foreach (Transform point in refs.uiLineTargets)
			{
				//Gets viewport position of cubes, makes depth 0 and uses that as origin point for line
				Vector3 viewPortPos = refs.gcRef.cam.WorldToViewportPoint(point.position);
				viewPortPos = new Vector3(viewPortPos.x, viewPortPos.y, 0);
				Vector3 lineOrigin = refs.gcRef.cam.ViewportToWorldPoint(viewPortPos);

				if (Physics.Linecast(lineOrigin, point.position, lineLayers))
				{
					ShowOrHideUI(true);
					return;
				}
				else
				{
					ShowOrHideUI(false);
				}
			}
		}

		private void AdjustLineRenderLength()
		{
			RaycastHit[] hits = SortedRayCasts();
			if (hits.Length > 0)
			{
				var distToRayOrigin = Vector3.Distance(this.transform.position, 
					uiRayOrigin.position);
				var newDist = distToRayOrigin - hits[0].distance;
				refs.uiLineRender.SetPosition(0, new Vector3(0, 0, (newDist * -100) + 
					elevatedLinePadding));
			}
			else refs.uiLineRender.SetPosition(0, Vector3.zero);
		}

		private RaycastHit[] SortedRayCasts()
		{
			RaycastHit[] hits = Physics.RaycastAll(uiRayOrigin.position, Vector3.down, uiRayDist, 
				lineRayLayers, QueryTriggerInteraction.Ignore);

			float[] hitDistances = new float[hits.Length];

			for (int hit = 0; hit < hitDistances.Length; hit++)
			{
				hitDistances[hit] = hits[hit].distance;
			}

			Array.Sort(hitDistances, hits);

			return hits;
		}

		private void HideUIForFinish()
		{
			hiddenForFinish = true;
			ShowOrHideUI(false);
		}

		public void ShowOrHideUI(bool value)
		{
			if (!cubeUIActive) return;
			refs.uiElement.SetActive(value);
			refs.uiLineRender.enabled = value;
		}

		private void CheckForMirror()
		{
			if (refs.turnCube != null)
			{
				if (refs.turnCube.isLeftTurning)
					refs.uiElement.transform.localScale = new Vector3(1, -1, 1);
			}
		}

		public void UpdateUIPos()
		{
			transform.position = refs.movCube.transform.position;
			transform.rotation = refs.effectorFace.transform.rotation;
			uiRayOrigin.transform.position = new Vector3(transform.parent.position.x,
				transform.parent.position.y + 2.95f, transform.parent.position.z);
		}
	}
}
