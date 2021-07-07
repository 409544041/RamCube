using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.General
{
	public class AllignCamToBilly : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float camMoveSpeed = 10f;
		[SerializeField] Transform camTarget;
		//Cache
		FinishEndSeqHandler finishEndSeq;

		private void Awake() 
		{
			finishEndSeq = FindObjectOfType<FinishEndSeqHandler>();
		}

		private void OnEnable() 
		{
			if (finishEndSeq != null)
			{
				finishEndSeq.onAlignCam += AllignRotation;
				finishEndSeq.onMoveCam += InitiateCamMove;
			} 
		}

		private void AllignRotation() 
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			transform.rotation = player.transform.rotation;
		}

		private void InitiateCamMove()
		{
			StartCoroutine(MoveCam());
		}

		private IEnumerator MoveCam()
		{
			float step = camMoveSpeed * Time.deltaTime;

			yield return new WaitForSeconds(.25f);

			while (Vector3.Distance(transform.position, camTarget.position) > 0.01f)
			{
				transform.position = Vector3.MoveTowards(transform.position, camTarget.position, step);
				yield return null;
			}

			transform.position = camTarget.position;
		}

		private void OnDisable()
		{
			if (finishEndSeq != null)
			{
				finishEndSeq.onAlignCam -= AllignRotation;
				finishEndSeq.onMoveCam -= InitiateCamMove;
			}
		}
	}
}
