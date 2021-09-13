using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeShrinker : MonoBehaviour
	{
		//Config parameters
		public float shrinkStep = 0f;
		public float timeStep = 0f;
		public MMFeedbacks shrinkFeedback;
		public float shrinkFeedbackDuration = 0f;
		public MeshRenderer mesh, shrinkMesh;

		//Cache
		LineRenderer laserLine = null;
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
			SetResetData();
			laserLine = GetComponent<FloorCube>().laserLine; 
			//Here instead of awake bc for when moveable becomes floor and components get added after awake
		}

		private void SetResetData()
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

			var cube = GetComponent<FloorCube>();
			var cubePos = cube.FetchGridPos();
			handler.FromFloorToShrunkDic(cubePos, cube);

			//Makes sure all values are reset in case this is the second shrink
			shrinkMesh.transform.position = resetPos;
			shrinkMesh.transform.rotation = resetRot;
			shrinkMesh.transform.localScale = resetScale;

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
