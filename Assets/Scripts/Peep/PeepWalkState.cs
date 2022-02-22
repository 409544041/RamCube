using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepWalkState : MonoBehaviour, IPeepBaseState
	{
		//Config parameters
		[SerializeField] float walkSpeed = 2, turnSpeed = 25;
		[SerializeField] float minPatrolPointDis = 1;

		//Cache
		PeepStateManager stateManager;

		//States
		Transform targetDest;
		int pathI = 1;
		Vector3 nextNodeDest = new Vector3 (float.PositiveInfinity, float.PositiveInfinity,
			float.PositiveInfinity);
		NavMeshPath path;
		NavMeshAgent agent;

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null) stateManager = psm;
			agent = stateManager.refs.agent;

			List<Transform> targets = new List<Transform>();

			foreach (var point in stateManager.pointManager.patrolPoints)
			{
				if (Vector3.Distance(transform.position, point.transform.position) > minPatrolPointDis)
					targets.Add(point.transform);
			}

			var i = Random.Range(0, targets.Count);
			targetDest = targets[i];

			agent.speed = walkSpeed;
			stateManager.refs.agent.destination = targetDest.position;
			path = new NavMeshPath();
			agent.CalculatePath(targetDest.position, path);
			pathI = 1; //not 0 because that is the player's current pos
			agent.isStopped = false;
		}

		public void StateUpdate(PeepStateManager psm)
		{
			MoveWithSmoothRotation();
			//somehow agent.move and agent.destination work good together. Probably shouldn't though.
		}

		public void StateFixedUpdate(PeepStateManager psm)
		{
		}

		private void MoveWithSmoothRotation()
		{
			if (path.corners == null || path.corners.Length == 0) return;	

			if (pathI >= path.corners.Length)
			{
				nextNodeDest = new Vector3(float.PositiveInfinity, float.PositiveInfinity,
					float.PositiveInfinity);
				agent.isStopped = true;

				if ((Object)stateManager.currentState != this) return;

				stateManager.refs.idleState.pointAction = 
					targetDest.GetComponent<IdlePointAction>().pointAction;
				stateManager.SwitchState(stateManager.refs.idleState);
			}
			else
			{
				nextNodeDest = path.corners[pathI];
			}

			if (nextNodeDest.x < float.PositiveInfinity) //just to check if we have valid data for the pos
			{
				var dir = nextNodeDest - agent.transform.position;
				var newDir = Vector3.RotateTowards(transform.forward, dir, turnSpeed * Time.deltaTime, 0.0f);
				var newRot = Quaternion.LookRotation(newDir);
				transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 2* Time.deltaTime);

				var distance = Vector3.Distance(agent.transform.position, nextNodeDest);
				if(distance > agent.radius + .1f)
				{
					var movement = transform.forward * walkSpeed * Time.deltaTime;
					agent.Move(movement);
				}
				else pathI++;
			}
		}
	}
}
