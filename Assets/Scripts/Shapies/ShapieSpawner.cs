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
		FinishCube finishCube;

		//States
		List<float> pushDegrees = new List<float> { 0, 45, 90, 135, 180, 225, 270, 315 };

		private void Awake()
		{
			finishCube = GetComponent<FinishCube>();
		}

		private void OnEnable()
		{
			if (finishCube != null) finishCube.onSpawnShapie += SpawnShapie;
		}

		private void SpawnShapie()
		{
			for (int i = 0; i < spawnAmount; i++)
			{
				int shapeIndex = UnityEngine.Random.Range(0, shapies.Length - 1);
				GameObject toSpawn = shapies[shapeIndex];
				int degreeIndex = UnityEngine.Random.Range(0, pushDegrees.Count - 1);
				Instantiate(toSpawn, transform.position, Quaternion.Euler(0f, pushDegrees[degreeIndex], 0f));
				pushDegrees.RemoveAt(degreeIndex);
			}
		}

		private void OnDisable()
		{
			if (finishCube != null) finishCube.onSpawnShapie -= SpawnShapie;
		}
	}
}
