using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeUIHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject uiElement = null;
		[SerializeField] Transform[] pointsToCheck = null;
		[SerializeField] LayerMask lineLayers;
		[SerializeField] float triggerDis = 0.3f;

		//Cache
		LineRenderer lRender = null;
		PlayerCubeMover mover = null;
		FloorCube floorCube = null;
		FinishCube finishCube = null;

		//States
		float disToPlayer = 0;
		bool hiddenForFinish = false;

		private void Awake() 
		{
			lRender = GetComponent<LineRenderer>();
			mover = FindObjectOfType<PlayerCubeMover>();
			floorCube = GetComponentInParent<FloorCube>();
			finishCube = FindObjectOfType<FinishCube>();
		}

		private void Start()
		{
			CheckForMirror();

			lRender.SetPosition(1, uiElement.transform.localPosition);
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
				(floorCube.transform.position, mover.transform.position);

			//The last part is to avoid bug when static cube becomes floor cube
			if (mover.isMoving || mover.isBoosting || mover.isTurning ||
				disToPlayer < triggerDis || floorCube.type == CubeTypes.Shrinking) 
			{
				ShowOrHideUI(false);
				return;
			} 

			foreach (Transform point in pointsToCheck)
			{
				//Gets viewport position of cubes, makes depth 0 and uses that as origin point for line
				Vector3 viewPortPos = Camera.main.WorldToViewportPoint(point.position);
				viewPortPos = new Vector3(viewPortPos.x, viewPortPos.y, 0);
				Vector3 lineOrigin = Camera.main.ViewportToWorldPoint(viewPortPos);

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
			uiElement.SetActive(value);
			lRender.enabled = value;
		}

		private void CheckForMirror()
		{
			TurningCube turner = GetComponentInParent<TurningCube>();

			if (turner)
			{
				if (turner.isLeftTurning)
					uiElement.transform.localScale = new Vector3(-1, 1, 1);
			}
		}
	}
}
