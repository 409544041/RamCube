using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class FloraSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool spawnFlora = true;
		[SerializeField] FloraIdentifier[] flora;
		[SerializeField] int[] spawnAmountWeight;
		[SerializeField] Vector2 minMaxBushSize, minMaxRockSize, minMaxMossSize;
		[SerializeField] bool generateOnStart = false;

		//States
		int spawnAmount = 0;

		private void Start() 
		{
			if (!spawnFlora) return;

			if (generateOnStart) SpawnFlora();
		}


		public void SpawnFlora() 
		{
			if (!spawnFlora) return;

			GetSpawnAmount();
			GenerateFlora();
		}

		private void GetSpawnAmount()
		{
			var amountIndex = Random.Range(0, spawnAmountWeight.Length);
			spawnAmount = spawnAmountWeight[amountIndex];	

			if (flora.Length < spawnAmount)
			{
				spawnAmount = flora.Length;
			}	
		}

		private void GenerateFlora()
		{
			for (int i = 0; i < spawnAmount; i++)
			{
				if (i == 0) 
				{
					var toSpawn = flora[Random.Range(0, flora.Length)];
					foreach (var mesh in toSpawn.floraMeshes)
					{
						mesh.enabled = true;
					}
					ApplyVariation(toSpawn);
					toSpawn.canSpawn = false;
				}
				else
				{
					List<FloraIdentifier> spawnables = new List<FloraIdentifier>();

					foreach (var flor in flora)
					{
						if (flor.canSpawn == true) spawnables.Add(flor);
					}

					var toSpawn = spawnables[Random.Range(0, spawnables.Count)];
					foreach (var mesh in toSpawn.floraMeshes)
					{
						mesh.enabled = true;
					}
					ApplyVariation(toSpawn);
					toSpawn.canSpawn = false;
				}
			}

			foreach (var flor in flora)
			{
				if (flor.canSpawn)
				{
					foreach (var mesh in flor.floraMeshes)
					{
						mesh.enabled = false;
					}
				}
			}
		}

		private void ApplyVariation(FloraIdentifier flor)
		{
			if (flor.floraType == FloraID.bush)
			{
				var scale = Random.Range(minMaxBushSize.x, minMaxBushSize.y);
				flor.transform.localScale = new Vector3 (scale, scale, scale);
			} 

			if (flor.floraType == FloraID.rock)
			{
				var scale = Random.Range(minMaxRockSize.x, minMaxRockSize.y);
				flor.transform.localScale = new Vector3(scale, scale, scale);

				float rot = Random.Range(0, 359);
				flor.transform.rotation = Quaternion.Euler(0, rot, 0);
			}

			if (flor.floraType == FloraID.moss)
			{
				var scale = Random.Range(minMaxMossSize.x, minMaxMossSize.y);
				flor.transform.localScale = new Vector3 (scale, scale, scale);
			}
		}
	}
}
