using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeUIHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] LayerMask lineLayers;
		[SerializeField] float triggerDis = 0.3f;
		[SerializeField] CubeRefHolder refs;

		//Cache
		PlayerCubeMover mover;
		FinishCube finishCube;

		//States
		float disToPlayer = 0;
		bool hiddenForFinish = false;

		private void Awake() 
		{
			mover = refs.gcRef.pRef.playerMover;
			finishCube = refs.gcRef.finishRef.finishCube;
		}

		private void Start()
		{
			CheckForMirror();

			refs.uiLineRender.SetPosition(1, refs.uiElement.transform.localPosition);
		}

		private void Update()
		{
			if (!finishCube.FetchFinishStatus()) ShowUICheck();
			if (finishCube.FetchFinishStatus() && !hiddenForFinish)	
				HideUIForFinish();
		}

		private void ShowUICheck()
		{
			disToPlayer = Vector3.Distance
				(refs.floorCube.transform.position, mover.transform.position);

			//The last part is to avoid bug when static cube becomes floor cube
			if (mover.isMoving || mover.isBoosting || mover.isTurning ||
				disToPlayer < triggerDis || refs.floorCube.type == CubeTypes.Shrinking) 
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

		private void HideUIForFinish()
		{
			hiddenForFinish = true;
			ShowOrHideUI(false);
		}

		private void ShowOrHideUI(bool value)
		{
			refs.uiElement.SetActive(value);
			refs.uiLineRender.enabled = value;
		}

		private void CheckForMirror()
		{
			if (refs.turnCube != null)
			{
				if (refs.turnCube.isLeftTurning)
					refs.uiElement.transform.localScale = new Vector3(-1, 1, 1);
			}
		}
	}
}
