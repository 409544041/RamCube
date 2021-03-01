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

		//Cache
		MeshRenderer mesh = null;
		MeshRenderer shrinkMesh = null;

		//States
		public bool hasShrunk { get; set; } = false;
		Vector3 resetPos;
		Quaternion resetRot;
		Vector3 resetScale;

		private void Start() 
		{
			MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
			mesh = meshes[0];
			shrinkMesh = meshes[1];

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

			hasShrunk = true;

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
