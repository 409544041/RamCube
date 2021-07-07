using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.Shapies
{
	public class ShapieSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject[] shapies = null;
		[SerializeField] int spawnAmount = 3;

		//Cache
		FinishEndSeqHandler finishEndSeq;

		//States
		List<float> pushDegrees = new List<float> { 0, 45, 90, 135, 180, 225, 270, 315 };

		private void Awake()
		{
			finishEndSeq = GetComponent<FinishEndSeqHandler>();
		}

		private void OnEnable()
		{
			if (finishEndSeq != null) finishEndSeq.onSpawnShapie += SpawnShapie;
		}

		private void SpawnShapie()
		{
			var spawnPos = new Vector3(transform.position.x,
				transform.position.y - .5f, transform.position.z);

			for (int i = 0; i < spawnAmount; i++)
			{
				int shapeIndex = Random.Range(0, shapies.Length - 1);
				GameObject toSpawn = shapies[shapeIndex];
				int degreeIndex = Random.Range(0, pushDegrees.Count - 1);
				Instantiate(toSpawn, spawnPos, Quaternion.Euler(0f, pushDegrees[degreeIndex], 0f));
				pushDegrees.RemoveAt(degreeIndex);
			}
		}

		private void OnDisable()
		{
			if (finishEndSeq != null) finishEndSeq.onSpawnShapie -= SpawnShapie;
		}
	}
}
