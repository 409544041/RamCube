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

		public event Action<FloorCube> onRecordStart;
		public event Action<FloorCube> onRecordStop;

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
			hasShrunk = true;
			Vector3 targetScale = new Vector3(0, 0, 0);

			onRecordStart(this);

			shrinkFeedback.Initialization();
			shrinkFeedback.PlayFeedbacks(); //playing through code instead of UnityEvent here due to floorcube component spawning for moveable cubes

			// for (int i = 0; i < (2.5 / shrinkStep); i++)
			// {
			// 	transform.localScale = 
			// 		Vector3.Lerp(transform.localScale, targetScale, shrinkStep);
			// 	yield return new WaitForSeconds(timeStep);
			// }

			yield return new WaitForSeconds(shrinkFeedbackDuration); //If shrink feedback is edited, edit this value to correspond to that

			onRecordStop(this);
		}

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
