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
		[SerializeField] Collider coll;
		[SerializeField] NavMeshObstacle navMeshOb;

		private void Start()
		{
			if (!meshRenderer.enabled)
			{
				if (coll != null) coll.enabled = false;
				if (navMeshOb != null) navMeshOb.enabled = false;
			}
		}
	}
}
