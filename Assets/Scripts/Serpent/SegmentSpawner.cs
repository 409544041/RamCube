using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentSpawner : MonoBehaviour
	{
		//States
		GameObject segmentToSpawn = null;

		//Cache
		FinishCube finishCube;

		//Actions, events, delegates etc
		public delegate GameObject GetSegDel();
		public GetSegDel onFetchSegmentToSpawn;

		private void Awake() 
		{
			finishCube = GetComponent<FinishCube>();
		}

		private void OnEnable() 
		{
			if (finishCube != null)
			{
				finishCube.onSetSegment += FetchSegmentToSpawn;
				finishCube.onSpawnSegment += SpawnSegment;
			} 
		}

		private void FetchSegmentToSpawn()
		{
			segmentToSpawn = onFetchSegmentToSpawn();
		}

		private void SpawnSegment()
		{
			Instantiate(segmentToSpawn, transform.position, Quaternion.identity);
			segmentToSpawn.transform.position = this.transform.position;
		}

		private void OnDisable()
		{
			if (finishCube != null)
			{
				finishCube.onSetSegment -= FetchSegmentToSpawn;
				finishCube.onSpawnSegment -= SpawnSegment;
			}
		}
	}
}
