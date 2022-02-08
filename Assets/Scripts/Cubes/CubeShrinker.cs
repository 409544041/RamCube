using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine.AI;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeShrinker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks shrinkFeedback;
		[SerializeField] MeshRenderer mesh, shrinkMesh;
		[SerializeField] LineRenderer laserLine = null;
		public NavMeshObstacle navMeshOb;


		//Cache
		CubeHandler handler = null;

		//States
		Vector3 resetPos;
		Quaternion resetRot;
		Vector3 resetScale;
		float totalFeedbackDur;

		private void Awake() 
		{
			handler = FindObjectOfType<CubeHandler>();
		}

		private void Start()
		{
			if (GetComponent<FloorCube>()) SetResetData();

			GetTotalFeedbackDur();
		}

		public void SetResetData()
		{
			resetPos = shrinkMesh.transform.position;
			resetRot = shrinkMesh.transform.rotation;
			resetScale = shrinkMesh.transform.localScale;
			shrinkMesh.enabled = false;
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

			mesh.enabled = false;
		}

		private IEnumerator ShrinkFloorCubes()
		{
			mesh.enabled = false;
			shrinkMesh.enabled = true;
			laserLine.enabled = false;
			navMeshOb.enabled = false;

			var staticCube = GetComponent<StaticCube>();
			if (staticCube) staticCube.SwitchFaces();

			// checking for floor bc it might be moveable that gets shrunk at end of level
			var floorCube = GetComponent<FloorCube>();
			if (floorCube)
			{
				var cubePos = floorCube.cubePoser.FetchGridPos();
				handler.FromFloorToShrunkDic(cubePos, floorCube);

				//Makes sure all values are reset in case this is the second shrink
				shrinkMesh.transform.position = resetPos;
				shrinkMesh.transform.rotation = resetRot;
				shrinkMesh.transform.localScale = resetScale;
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

			shrinkMesh.enabled = false;
		}

		public void EnableMesh()
		{
			mesh.enabled = true;
		}
	}
}
