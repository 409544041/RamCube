﻿using System;
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
		[SerializeField] MMFeedbacks shrinkFeedback, markShrinkFeedback;
		[SerializeField] CubeRefHolder refs;
		[SerializeField] Renderer wallMesh;

		//Cache
		CubeHandler handler; 

		//States
		Vector3 resetPos;
		Quaternion resetRot;
		Vector3 resetScale;
		float totalFeedbackDur;
		public bool hasShrunk { get; set; } = false;

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
			resetPos = refs.shrinkMesh.transform.localPosition;
			resetRot = refs.shrinkMesh.transform.localRotation;
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

		public void ShrinkGroundMarks()
		{
			if (markShrinkFeedback == null) return;
			
			markShrinkFeedback.Initialization();
			markShrinkFeedback.PlayFeedbacks();
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
			refs.laserDottedLine.enabled = false;
			hasShrunk = true;

			if (refs.effectorFace != null)
			{
				refs.effectorFace.enabled = false;
				refs.effectorShrinkingFace.enabled = true;
				if (refs.movEffector != null) refs.effectorShrinkingFace.transform.parent =
					refs.shrinkMesh.transform;
			}

			// checking for floor bc it might be moveable that gets shrunk at end of level
			if (refs.floorCube != null)
			{
				var cubePos = refs.cubePos.FetchGridPos();
				handler.FromFloorToShrunkDic(cubePos, refs.floorCube);

				//Makes sure all values are reset in case this is the second shrink
				ResetTransform();
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

		public void ResetTransform()
		{
			refs.shrinkMesh.transform.localPosition = resetPos;
			refs.shrinkMesh.transform.localRotation = resetRot;
			refs.shrinkMesh.transform.localScale = resetScale;
		}

		public void EnableMesh()
		{
			refs.mesh.enabled = true;
		}
	}
}
