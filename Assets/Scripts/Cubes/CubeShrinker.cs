using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.MoveableCubes;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeShrinker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float shrinkStep = 0f;
		[SerializeField] float timeStep = 0f;
		[SerializeField] MMFeedbacks shrinkFeedback;
		[SerializeField] float shrinkFeedbackDuration = 0f;
		[SerializeField] MeshRenderer mesh, shrinkMesh;
		[SerializeField] LineRenderer laserLine = null;


		//Cache
		CubeHandler handler = null;

		//States
		Vector3 resetPos;
		Quaternion resetRot;
		Vector3 resetScale;

		private void Awake() 
		{
			handler = FindObjectOfType<CubeHandler>();
		}

		private void Start()
		{
			if (GetComponent<FloorCube>()) SetResetData();
		}

		public void SetResetData()
		{
			resetPos = shrinkMesh.transform.position;
			resetRot = shrinkMesh.transform.rotation;
			resetScale = shrinkMesh.transform.localScale;
			shrinkMesh.enabled = false;
		}

		public void StartShrinking()
		{
			StartCoroutine(Shrink());
		}

		private IEnumerator Shrink()
		{
			mesh.enabled = false;
			shrinkMesh.enabled = true;
			laserLine.enabled = false;

			// checking for floor bc it might be moveable that gets shrunk at end of level
			var floorCube = GetComponent<FloorCube>();
			if (floorCube)
			{
				var cubePos = floorCube.FetchGridPos();
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

			yield return new WaitForSeconds(shrinkFeedbackDuration);
			//----- TO DO: If shrink feedback is edited, edit this value to correspond to that

			shrinkMesh.enabled = false;
		}

		public void EnableMesh()
		{
			mesh.enabled = true;
		}
	}
}
