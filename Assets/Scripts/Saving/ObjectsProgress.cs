using Qbism.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class ObjectsProgress : MonoBehaviour
	{
		//States
		public List<ObjectStatusData> objectsDataList { get; private set; }
			= new List<ObjectStatusData>();

		private void Awake()
		{
			BuildObjectsList();
			LoadObjectsData();
		}

		private void BuildObjectsList()
		{
			for (int i = 0; i < E_ObjectsGameplayData.CountEntities; i++)
			{
				ObjectStatusData newData = new ObjectStatusData();
				objectsDataList.Add(newData);
			}
		}

		private void LoadObjectsData()
		{
			ProgData data = SavingSystem.LoadProgData();
			if (data == null) return;

			for (int i = 0; i < E_ObjectsGameplayData.CountEntities; i++)
			{
				E_ObjectsGameplayData.GetEntity(i).f_ObjectFound = data.savedObjectsData[i].found;
				E_ObjectsGameplayData.GetEntity(i).f_ObjectReturned = data.savedObjectsData[i].returned;
			}
		}

		public void SaveObjectsData()
		{
			for (int i = 0; i < E_ObjectsGameplayData.CountEntities; i++)
			{
				var objEntity = E_ObjectsGameplayData.GetEntity(i);
				var objData = objectsDataList[i];

				objData.found = objEntity.f_ObjectFound;
				objData.returned = objEntity.f_ObjectReturned;				
			}
		}

		public void AddObjectToDatabase()
		{
			ObjectSpawner objSpawner = FindObjectOfType<ObjectSpawner>();

			var objEntity = E_ObjectsGameplayData.FindEntity(entity =>
				entity.f_Object == objSpawner.objectToSpawn);

			objEntity.f_ObjectFound = true;
		}

		public void WipeObjectsData()
		{
			for (int i = 0; i < E_ObjectsGameplayData.CountEntities; i++)
			{
				var objEntity = E_ObjectsGameplayData.GetEntity(i);
				objEntity.f_ObjectFound = false;
				objEntity.f_ObjectReturned = false;
			}
		}
	}
}
