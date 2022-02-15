using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.Saving
{
	public class SerpentProgress : MonoBehaviour
	{
		//Cache
		SegmentSpawner segSpawner = null;

		//States
		public List<bool> serpentDataList;

		private void Awake() 
		{
			BuildSerpentList();
			LoadSerpentData();
		}

		private void BuildSerpentList()
		{
			serpentDataList = new List<bool>();

			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				serpentDataList.Add(false);
			}
		}

		public void LoadSerpentData()
		{
			ProgData data = SavingSystem.LoadProgData();
			if (data == null) return;

			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				E_SegmentsGameplayData.GetEntity(i).f_Rescued = data.savedSerpentDataList[i];
			}
		}

		public void SaveSerpentData()
		{
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				serpentDataList[i] = E_SegmentsGameplayData.GetEntity(i).f_Rescued;
			}
		}

		public void AddSegment()
		{
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				if(E_SegmentsGameplayData.GetEntity(i).f_Rescued == true) continue;
				else
				{
					E_SegmentsGameplayData.GetEntity(i).f_Rescued = true;
					return;
				}
			}
		}

		private GameObject FetchSegmentToSpawn()
		{
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				if(E_SegmentsGameplayData.GetEntity(i).f_Rescued == false)
				{
					GameObject nextSegmentToUnlock;

					if (E_SegmentsGameplayData.GetEntity(i).f_Segment.f_SpawnPrefab != null)
						nextSegmentToUnlock =
						(GameObject)E_SegmentsGameplayData.GetEntity(i).f_Segment.f_SpawnPrefab;

					else nextSegmentToUnlock =
							(GameObject)E_SegmentsGameplayData.GetEntity(i).f_Segment.f_Prefab;
					
					return nextSegmentToUnlock;
				}
			}

			Debug.LogError("Couldn't find next segment to unlock.");
			return null;
		}

		public void FixGameplayDelegateLinks()
		{
			segSpawner = FindObjectOfType<SegmentSpawner>();
			if (segSpawner != null) segSpawner.onFetchSegmentToSpawn += FetchSegmentToSpawn;
		}

		private void OnDisable()
		{
			if (segSpawner != null) segSpawner.onFetchSegmentToSpawn -= FetchSegmentToSpawn;
		}
	}
}
