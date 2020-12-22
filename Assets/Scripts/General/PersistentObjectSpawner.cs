using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class PersistentObjectSpawner : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject persistentObjectPrefab;
		
		static bool hasSpawned = false; //static means it'll be saved even if it's another instance
		
		private void Awake()
		{
			if(hasSpawned) return;

			SpawnPersistentObjects();

			hasSpawned = true;
		}

		private void SpawnPersistentObjects()
		{
			GameObject persistentObject = Instantiate(persistentObjectPrefab);
			DontDestroyOnLoad(persistentObject);
		}
	}
}

