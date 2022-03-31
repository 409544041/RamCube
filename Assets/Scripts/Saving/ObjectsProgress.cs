using Qbism.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class ObjectsProgress : MonoBehaviour
	{
		//Config parameters
		[SerializeField] PersistentRefHolder persRef;

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
				E_ObjectsGameplayData.GetEntity(i).f_QuestMarkerShown = 
					data.savedObjectsData[i].questMarkerShown;
				E_ObjectsGameplayData.GetEntity(i).f_ObjectQuestGiven = data.savedObjectsData[i].questGiven;
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

				objData.questMarkerShown = objEntity.f_QuestMarkerShown;
				objData.questGiven = objEntity.f_ObjectQuestGiven;
				objData.found = objEntity.f_ObjectFound;
				objData.returned = objEntity.f_ObjectReturned;				
			}
		}

		public void AddObjectToDatabase()
		{
			var objSpawner = persRef.gcRef.finishRef.objSpawner;

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
