using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Environment
{
	public class FloraSpawnChecker : MonoBehaviour
	{
		//Config Parameters
		[SerializeField] MeshRenderer meshRenderer;
		public Collider coll;
		public NavMeshObstacle navMeshOb;
		[SerializeField] GameObject hidePoint;

		private void Start()
		{
			if (!meshRenderer.enabled)
			{
				if (coll != null) coll.enabled = false;
				if (navMeshOb != null) navMeshOb.enabled = false;
				if (hidePoint != null) hidePoint.SetActive(false);
			}
		}
	}
}
