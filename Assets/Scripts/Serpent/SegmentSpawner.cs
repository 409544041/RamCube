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
		FinishEndSeqHandler finishEndSeq;
		FinishCube finish;

		//Actions, events, delegates etc
		public delegate GameObject GetSegDel();
		public GetSegDel onFetchSegmentToSpawn;

		private void Awake() 
		{
			finish = GetComponent<FinishCube>();
			finishEndSeq = GetComponent<FinishEndSeqHandler>();
		}

		private void OnEnable() 
		{
			if (finish != null) finish.onSetSegment += FetchSegmentToSpawn;
			if (finishEndSeq != null) finishEndSeq.onSpawnSegment += SpawnSegment;
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
			if (finish != null) finish.onSetSegment -= FetchSegmentToSpawn;
			if (finishEndSeq != null) finishEndSeq.onSpawnSegment -= SpawnSegment;
		}
	}
}
