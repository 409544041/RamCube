using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FloorCube : MonoBehaviour
	{
		//Config parameters
		public CubeTypes type = CubeTypes.Shrinking;
		public float shrinkStep = 0f;
		public float timeStep = 0f;
		public MMFeedbacks shrinkFeedback;
		public float shrinkFeedbackDuration = 0f;

		//States
		public bool hasShrunk { get; set; } = false;
		public bool isFindable { get; set; } = true;
		//public bool isShrinking { get; set; } = false;

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}

		private void Update() 
		{
			CheckForVisualDisabling();
		}

		public void StartShrinking()
		{
			StartCoroutine(Shrink());
		}

		private IEnumerator Shrink()
		{
			//isShrinking = true; //----- TO DO: Wait on MM respose. Fix this.
			hasShrunk = true;

			MMFeedbackPosition[] posFeedbacks =
				shrinkFeedback.GetComponents<MMFeedbackPosition>();
			foreach (MMFeedbackPosition posFeedback in posFeedbacks)
			{
				//This makes sure when shrinking for a second time the initial position doesn't magically changes (bug in MM?)
				if(posFeedback.Label == "Position Upwards")
				{
					posFeedback.InitialPosition = new Vector3(0, 0, 0);
					posFeedback.DestinationPosition = new Vector3(0, 1, 0);
				}

				if(posFeedback.Label =="Position Downwards")
				{
					posFeedback.InitialPosition = new Vector3(0, 1, 0);
					posFeedback.DestinationPosition = new Vector3(0, -1, 0);
				}
			}

			shrinkFeedback.Initialization();
			shrinkFeedback.PlayFeedbacks(); 

			yield return new WaitForSeconds(shrinkFeedbackDuration); 
			//----- TO DO: If shrink feedback is edited, edit this value to correspond to that

			//isShrinking = false;
		}

		// public void StopShrinking()
		// {
		// 	print("Stopping Shrinking");
		// 	shrinkFeedback.StopFeedbacks();
		// 	shrinkFeedback.ResetFeedbacks();
		// 	shrinkFeedback.enabled = false;
		// 	isShrinking = false;
		// }

		private void CheckForVisualDisabling()
		{
			if(type != CubeTypes.Shrinking) return;

			if(transform.localScale.x < .1f) 
				GetComponent<MeshRenderer>().enabled = false;
			else GetComponent<MeshRenderer>().enabled = true;
		}

		public CubeTypes FetchType()
		{
			return type;
		}
	}
}
