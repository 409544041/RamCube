using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Animator animator;
		[SerializeField] Rigidbody rb;

		//States
		Vector3 prevPos;
		float currentSpeed;

		private void Update()
		{
			CalculateCurrentSpeed();
			animator.SetFloat("MoveSpeed", currentSpeed);
		}

		private void CalculateCurrentSpeed()
		{
			var currentMovement = transform.position - prevPos;
			currentSpeed = currentMovement.magnitude / Time.deltaTime;
			prevPos = transform.position;
		}
	}
}
