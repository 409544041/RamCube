using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepNavPointManager : MonoBehaviour
	{
		//Cache
		public GameObject[] patrolPoints { get; private set; }
		public GameObject[] hidePoints { get; private set; }
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
				peep.GetComponent<PeepStateManager>().pointManager = this;
			}

			StartCoroutine(GetHidePoints()); 
		}

		private IEnumerator GetHidePoints()
		{
			//doing this with delay to give florachecker time to turn off hidepoints
			yield return new WaitForSeconds(.1f);

			hidePoints = GameObject.FindGameObjectsWithTag(("HidePoint"));
		}
	}
}