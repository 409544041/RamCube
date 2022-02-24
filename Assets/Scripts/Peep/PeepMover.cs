using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepMover : MonoBehaviour
	{
		//Config parameters
		[SerializeField] PeepRefHolder refs;
		[SerializeField] float turnSpeed = 200;

		//States
		int pathI = 1;
		Vector3 nextNodeDest = new Vector3(float.PositiveInfinity, float.PositiveInfinity,
			float.PositiveInfinity);
		NavMeshPath path;
		IPeepMovement moverState;

		public void PrepareMove(Transform dest, IPeepMovement moveState)
		{
			moverState = moveState;
			path = new NavMeshPath();
			refs.agent.CalculatePath(dest.position, path);
			pathI = 1; //not 0 because that is the player's current pos
			refs.agent.isStopped = false;
		}
		public void MoveWithSmoothRotation(float moveSpeed)
		{
			if (path.corners == null || path.corners.Length == 0) return;

			if (pathI >= path.corners.Length)
			{
				nextNodeDest = new Vector3(float.PositiveInfinity, float.PositiveInfinity,
					float.PositiveInfinity);
				refs.agent.isStopped = true;

				moverState.DestinationReached();
			}
			else
			{
				nextNodeDest = path.corners[pathI];
			}

			if (nextNodeDest.x < float.PositiveInfinity) //just to check if we have valid data for the pos
			{
				var dir = nextNodeDest - refs.agent.transform.position;
				var newDir = Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f);
				var newRot = Quaternion.LookRotation(newDir);
				transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 2 * Time.deltaTime);

				var distance = Vector3.Distance(refs.agent.transform.position, nextNodeDest);
				if (distance > refs.agent.radius + .1f)
				{
					var movement = transform.forward * moveSpeed * Time.deltaTime;
					refs.agent.Move(movement);
				}
				else pathI++;
			}
		}
	}
}
