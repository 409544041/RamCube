using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using Pathfinding;

namespace Qbism.Cubes
{
	public class CubeShrinker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks shrinkFeedback;
		[SerializeField] CubeRefHolder refs;
		[SerializeField] Renderer wallMesh;

		//Cache
		CubeHandler handler; 

		//States
		Vector3 resetPos;
		Quaternion resetRot;
		Vector3 resetScale;
		float totalFeedbackDur;

		private void Awake()
		{
			if (refs!= null) handler = refs.gcRef.glRef.cubeHandler;
		}

		private void Start()
		{
			if (refs != null) SetResetData();

			GetTotalFeedbackDur();
		}

		public void SetResetData()
		{
			resetPos = refs.shrinkMesh.transform.position;
			resetRot = refs.shrinkMesh.transform.rotation;
			resetScale = refs.shrinkMesh.transform.localScale;
			refs.shrinkMesh.enabled = false;
		}

		private void GetTotalFeedbackDur()
		{
			var feedbacks = shrinkFeedback.GetComponents<MMFeedback>();

			for (int i = 0; i < feedbacks.Length; i++)
			{
				totalFeedbackDur += feedbacks[i].FeedbackDuration;
			}
		}

		public void StartShrinking()
		{
			if (gameObject.tag == "Wall")
				StartCoroutine(ShrinkWalls());

			else StartCoroutine(ShrinkFloorCubes()); 			
		}

		private IEnumerator ShrinkWalls ()
		{
			shrinkFeedback.Initialization();
			shrinkFeedback.PlayFeedbacks();

			yield return new WaitForSeconds(totalFeedbackDur);

			wallMesh.enabled = false;
		}

		private IEnumerator ShrinkFloorCubes()
		{
			refs.mesh.enabled = false;
			refs.shrinkMesh.enabled = true;
			refs.lineRender.enabled = false;

			if (refs.staticCube != null) refs.staticCube.SwitchFaces();

			// checking for floor bc it might be moveable that gets shrunk at end of level
			if (refs.floorCube != null)
			{
				var cubePos = refs.cubePos.FetchGridPos();
				handler.FromFloorToShrunkDic(cubePos, refs.floorCube);

				//Makes sure all values are reset in case this is the second shrink
				refs.shrinkMesh.transform.position = resetPos;
				refs.shrinkMesh.transform.rotation = resetRot;
				refs.shrinkMesh.transform.localScale = resetScale;
			}

			MMFeedbackPosition[] posFeedbacks =
				shrinkFeedback.GetComponents<MMFeedbackPosition>();
			foreach (MMFeedbackPosition posFeedback in posFeedbacks)
			{
				//This makes sure when shrinking for a second time the initial position doesn't magically changes (bug in MM?)
				if (posFeedback.Label == "Position Upwards")
				{
					posFeedback.InitialPosition = new Vector3(0, 0, 0);
					posFeedback.DestinationPosition = new Vector3(0, 1, 0);
				}

				if (posFeedback.Label == "Position Downwards")
				{
					posFeedback.InitialPosition = new Vector3(0, 1, 0);
					posFeedback.DestinationPosition = new Vector3(0, -1, 0);
				}
			}

			shrinkFeedback.Initialization();
			shrinkFeedback.PlayFeedbacks();

			yield return new WaitForSeconds(totalFeedbackDur);

			refs.shrinkMesh.enabled = false;
		}

		public void EnableMesh()
		{
			refs.mesh.enabled = true;
		}
	}
}
