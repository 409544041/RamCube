using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	[ExecuteAlways]
	public class FloraSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool spawnFlora = true;
		[SerializeField] FloraIdentifier[] flora;
		[SerializeField] int[] spawnAmountWeight;
		[SerializeField] Vector2 minMaxBushSize, minMaxRockSize, minMaxMossSize;
		[SerializeField] MeshRenderer dripMesh;

		//Cache
		public BiomeOverwriter bOverwriter { get; set; }

		//States
		int spawnAmount = 0;

		List<FloraIdentifier> floraList = new List<FloraIdentifier>();

		private void Start() 
		{	
			if (bOverwriter != null && bOverwriter.respawnFloraVariety)
			{
				if (dripMesh != null)
				{
					if (dripMesh.enabled == true) SpawnFlora();
				}
				else SpawnFlora();
			}
		}

		public void SpawnFlora() 
		{
			DespawnFlora();
			
			if (spawnFlora) 
			{
				AddFloraToFloraList();
				GetSpawnAmount();
				GenerateFlora();
			}
		}

		private void GetSpawnAmount()
		{
			var amountIndex = Random.Range(0, spawnAmountWeight.Length);
			spawnAmount = spawnAmountWeight[amountIndex];	

			if (floraList.Count < spawnAmount)
			{
				spawnAmount = floraList.Count;
			}	
		}

		private void GenerateFlora()
		{
			for (int i = 0; i < spawnAmount; i++)
			{
				var toSpawn = floraList[Random.Range(0, floraList.Count)];

				foreach (var flor in floraList)
				{
					ToggleMeshesAndColliders(flor, true);
				}

				ApplyVariation(toSpawn);
				toSpawn.canSpawn = false;
				floraList.Remove(toSpawn);
			}

			foreach (var flor in flora)
			{
				if (flor.canSpawn)
				{
					ToggleMeshesAndColliders(flor, false);
				}
			}
		}

		public void DespawnFlora()
		{
			foreach (var flor in flora)
			{
				ToggleMeshesAndColliders(flor, false);
			}
		}

		private void ApplyVariation(FloraIdentifier flor)
		{
			if (flor.floraType == FloraID.bush)
			{
				var scale = Random.Range(minMaxBushSize.x, minMaxBushSize.y);
				flor.transform.localScale = new Vector3 (scale, scale, scale);
			} 

			if (flor.floraType == FloraID.tree)
			{
				float rot = Random.Range(0, 359);
				flor.transform.rotation = Quaternion.Euler(0, rot, 0);
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

		private void AddFloraToFloraList()
		{
			foreach (var flor in flora)
			{
				if (flor.canSpawn) floraList.Add(flor);
				else ToggleMeshesAndColliders(flor, false);
			}
		}

		private void ToggleMeshesAndColliders(FloraIdentifier flor, bool value)
		{
			foreach (var mesh in flor.floraMeshes)
			{
				mesh.enabled = value;
			}

			foreach (var coll in flor.colliders)
			{
				coll.enabled = value;
			}

			if (flor.navMeshOb != null) flor.navMeshOb.enabled = value;
		}
	}
}
