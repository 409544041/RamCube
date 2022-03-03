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
		public Collider coll;
		public NavmeshCut nmCutter;

		private void Start()
		{
			if (!meshRenderer.enabled)
			{
				if (coll != null) coll.enabled = false;
				if (nmCutter != null) nmCutter.enabled = false;
			}
		}
	}
}
