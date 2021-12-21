using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float segSpawnY, headSegSpawnY;
		[SerializeField] float spawnDegrees = 135, headSpawnDegrees = 110;


		//States
		GameObject segmentToSpawn = null;

		//Cache
		FinishEndSeqHandler finishEndSeq;
		FinishCube finish;

		//Actions, events, delegates etc
		public Func<GameObject> onFetchSegmentToSpawn;

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
			bool isMother = segmentToSpawn.GetComponent<SegmentIdentifier>().segmentID ==
				SegmentIDs.segment_head;

			float spawnY = segSpawnY; float spawnDegs = spawnDegrees;

			if (isMother)
            {
				spawnY = headSegSpawnY;
				spawnDegs = headSpawnDegrees;
            }

			Vector3 spawnPos = new Vector3(transform.position.x, spawnY, transform.position.z);
			GameObject spawnedSegment = Instantiate(segmentToSpawn, spawnPos, 
				Quaternion.Euler(0f, spawnDegs, 0f));

			segmentToSpawn.transform.position = transform.position;

			if (isMother) spawnedSegment.GetComponentInChildren<MotherDragonAnimator>().
					Spawn(headSpawnDegrees);
			else spawnedSegment.GetComponentInChildren<SegmentAnimator>().Spawn();
		}

		private void OnDisable()
		{
			if (finish != null) finish.onSetSegment -= FetchSegmentToSpawn;
			if (finishEndSeq != null) finishEndSeq.onSpawnSegment -= SpawnSegment;
		}
	}
}
