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

		public void SetSegmentToSpawn()
		{
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				if (E_SegmentsGameplayData.GetEntity(i).f_Rescued == false)
				{
					if (E_SegmentsGameplayData.GetEntity(i).f_Segment.f_SpawnPrefab != null)
						segmentToSpawn =
						(GameObject)E_SegmentsGameplayData.GetEntity(i).f_Segment.f_SpawnPrefab;

					else segmentToSpawn =
							(GameObject)E_SegmentsGameplayData.GetEntity(i).f_Segment.f_Prefab;
					return;
				}
			}
		}

		public void SpawnSegment()
		{
			bool isMother = segmentToSpawn.GetComponent<M_Segments>().f_name == "segment_head";

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
	}
}
