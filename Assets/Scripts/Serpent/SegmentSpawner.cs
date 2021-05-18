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
			GameObject spawnedSegment = Instantiate(segmentToSpawn, transform.position, 
				Quaternion.Euler(0f, 145f, 0f));
			segmentToSpawn.transform.position = transform.position;
			spawnedSegment.GetComponentInChildren<SegmentAnimator>().Spawn();
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
