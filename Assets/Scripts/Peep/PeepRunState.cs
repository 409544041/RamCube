using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Qbism.Environment;

namespace Qbism.Peep
{
	public class PeepRunState : MonoBehaviour, IPeepBaseState
	{
		//Config parameters
		[SerializeField] float runSpeed = 4;

		//Cache
		PeepStateManager stateManager;

		//States
		Transform currentTarget; // just for easy reading in debug inspector

		public void StateEnter(PeepStateManager psm)
		{
			if (stateManager == null) stateManager = psm;
			// trigger startled anim 

			var points = SortHidePointsByDistance();
			StartCoroutine(SetNavTarget(points));
			// trigger running anim
		}

		public void StateUpdate(PeepStateManager psm)
		{

		}

		private void OnTriggerEnter(Collider other)
		{
			//if trigger = house || bush > shrinking juice to make it look like they shrank into the house
		}

		private GameObject[] SortHidePointsByDistance()
		{
			var hidePoints = stateManager.pointManager.hidePoints;
			float[] pointNavDistances = new float[hidePoints.Length];

			for (int i = 0; i < hidePoints.Length; i++)
			{
				float distToPoint = Vector3.Distance(transform.position, hidePoints[i].transform.position);
				pointNavDistances[i] = distToPoint;
			}

			Array.Sort(pointNavDistances, hidePoints);
			return hidePoints;
		}

		private IEnumerator SetNavTarget(GameObject[] points)
		{
			bool pathFound = false;

			for (int i = 0; i < points.Length; i++)
			{
				var path = new NavMeshPath();
				var pointChecker = points[i].GetComponentInParent<FloraSpawnChecker>();

				pointChecker.navMeshOb.carving = false;
				yield return null; //this to ensure the carving is actually turned off before the next bit

				if (stateManager.refs.agent.CalculatePath(points[i].transform.position, path) 
					&& pathFound == false)
				{
					stateManager.refs.agent.speed = runSpeed;
					currentTarget = points[i].transform;
					stateManager.refs.agent.destination = currentTarget.position;
					pointChecker.coll.enabled = true;
					pathFound = true;
					continue;
				}

				// disables the hide collider from other hidepoints that isn't the path hidepoint
				pointChecker.coll.enabled = false;
				pointChecker.navMeshOb.carving = true;
			}
		}
	}
}
