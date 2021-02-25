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

		private void Start() 
		{
			MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
			mesh = meshes[0];
			shrinkMesh = meshes[1];
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

			Vector3 resetPos = shrinkMesh.transform.position;
			Quaternion resetRot = shrinkMesh.transform.rotation;
			Vector3 resetScale = shrinkMesh.transform.localScale;

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
			shrinkMesh.transform.position = resetPos;
			shrinkMesh.transform.rotation = resetRot;
			shrinkMesh.transform.localScale = resetScale;
		}

		public void EnableMesh()
		{
			mesh.enabled = true;
		}
	}
}
