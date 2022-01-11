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
		[SerializeField] AnimatorOverrideController[] eyeOverriders, mouthOverriders;
		[SerializeField] Material[] mats;
		[SerializeField] int spawnAmount = 3;
		[SerializeField] float shapieSpawnY = -.5f;
		[SerializeField] float[] pushDegrees;

		//Cache
		FinishEndSeqHandler finishEndSeq;

		//States
		List<float> pushDegreesList = new List<float>();

		private void Awake()
		{
			finishEndSeq = GetComponent<FinishEndSeqHandler>();
		}

		private void Start() 
		{
			for (int i = 0; i < pushDegrees.Length; i++)
			{
				pushDegreesList.Add(pushDegrees[i]);
			}
		}

		private void OnEnable()
		{
			if (finishEndSeq != null) finishEndSeq.onSpawnShapie += SpawnShapie;
		}

		private void SpawnShapie()
		{
			var spawnPos = new Vector3(transform.position.x,
				shapieSpawnY, transform.position.z);

			for (int i = 0; i < spawnAmount; i++)
			{
				GameObject toSpawn = shapies[Random.Range(0, shapies.Length)];

				var refs = toSpawn.GetComponent<ShapieRefHolder>();

				refs.eyesAnimator.runtimeAnimatorController = 
					eyeOverriders[Random.Range(0, eyeOverriders.Length)];

				refs.mouthAnimator.runtimeAnimatorController =
					mouthOverriders[Random.Range(0, mouthOverriders.Length)];

				refs.bodyMesh.material = mats[Random.Range(0, mats.Length)];

				int degreeIndex = Random.Range(0, pushDegreesList.Count);
				Instantiate(toSpawn, spawnPos, Quaternion.Euler(0f, pushDegreesList[degreeIndex], 0f));
				pushDegreesList.RemoveAt(degreeIndex);
			}
		}

		private void OnDisable()
		{
			if (finishEndSeq != null) finishEndSeq.onSpawnShapie -= SpawnShapie;
		}
	}
}
