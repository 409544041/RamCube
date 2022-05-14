using Qbism.Cubes;
using Qbism.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class ObjectSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float objSpawnY;
		[SerializeField] FinishRefHolder fRef;

		//States
		public E_Objects objectToSpawn { get; private set; }

		public void SetObjectToSpawn()
		{
			var currenPin = fRef.gcRef.persRef.progHandler.currentPin;
			var currentLevelEntity = E_LevelData.FindEntity(entity =>
				entity.f_Pin == currenPin);

			if (currentLevelEntity.f_ForceObject != null)
				objectToSpawn = currentLevelEntity.f_ForceObject;
			else SemiRandomSpawnObject();
		}

		private void SemiRandomSpawnObject()
		{
			List<E_Objects> spawnables = new List<E_Objects>();
			List<E_Objects> backupSpawnables = new List<E_Objects>();

			for (int i = 0; i < E_ObjectsGameplayData.CountEntities; i++)
			{
				var objEntity = E_ObjectsGameplayData.GetEntity(i);

				if (objEntity.f_ObjectFound) continue;

				var segEntity = E_SegmentsGameplayData.FindEntity(entity =>
					entity.f_Segment == objEntity.f_Object.f_Owner);

				if (segEntity.f_Rescued) spawnables.Add(objEntity.f_Object);
				else backupSpawnables.Add(objEntity.f_Object);
			}

			if (spawnables.Count > 0)
			{
				int i = Random.Range(0, spawnables.Count);
				objectToSpawn = spawnables[i];
			}
			else
			{
				int i = Random.Range(0, backupSpawnables.Count);
				objectToSpawn = backupSpawnables[i];
			}
		}

		public void SpawnObject()
		{
			Vector3 spawnPos = new Vector3(transform.position.x, objSpawnY, transform.position.z);
			GameObject spawnedObj = Instantiate(objectToSpawn.f_Prefab, spawnPos, Quaternion.identity);
		}
	}
}
