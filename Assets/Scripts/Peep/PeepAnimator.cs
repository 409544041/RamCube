using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] PeepRefHolder refs;

		//States
		float moveSpeed;
		float yRotDelta;
		float yRotCurrent;

		private void Update()
		{
			moveSpeed = refs.aiRich.velocity.magnitude;
			refs.animator.SetFloat("MoveSpeed", moveSpeed);
			CalculateRotation();
			refs.animator.SetFloat("Rotation", yRotDelta);
		}

		private void CalculateRotation()
		{
			var yRotPrev = yRotCurrent;
			yRotCurrent = transform.rotation.y;
			yRotDelta = yRotCurrent - yRotPrev;			
		}

		public void TriggerAnim(string animTrigger)
		{
			refs.animator.SetTrigger(animTrigger);
		}

		public void SetAnimLayerValue(int layerIndex, int value)
		{
			refs.animator.SetLayerWeight(layerIndex, value);
		}
	}
}
