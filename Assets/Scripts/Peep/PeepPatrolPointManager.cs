using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepPatrolPointManager : MonoBehaviour
	{
		//Cache
		public GameObject[] patrolPoints { get; private set; }
		public GameObject[] peeps { get; private set; }

		private void Awake()
		{
			patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
			peeps = GameObject.FindGameObjectsWithTag("Peep");
		}

		private void Start()
		{
			foreach (var peep in peeps)
			{
				peep.GetComponent<PeepWalkState>().pointManager = this;
			}
		}
	}
}
