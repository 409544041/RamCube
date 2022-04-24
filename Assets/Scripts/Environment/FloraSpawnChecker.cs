using Pathfinding;
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

		private void Start()
		{
			if (!meshRenderer.enabled)
			{
				if (coll != null) coll.enabled = false;
			}
			else
			{
				if (coll != null) coll.enabled = true;
			}
		}
	}
}
