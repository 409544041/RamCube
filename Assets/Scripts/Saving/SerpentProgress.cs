using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.Saving
{
	public class SerpentProgress : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int serpentLength = 20;
		public GameObject[] segments;

		//Cache
		SerpentSegmentHandler[] serpSegHandlers = null;
		SerpentScreenSplineHandler serpSplineHandler = null;
		SegmentSpawner segSpawner = null;

		//States
		public List<bool> serpentDataList;
		public GameObject nextSegmentToUnlock;

		private void Awake() 
		{
			BuildSerpentList();
			// LoadSerpentData();
		}

		private void BuildSerpentList()
		{
			for (int i = 0; i < serpentLength; i++)
			{
				serpentDataList.Add(false);
			}
		}

		public void LoadSerpentData()
		{
			ProgData data = SavingSystem.LoadProgData();
			serpentDataList = data.savedSerpentDataList;
		}

		public void AddSegment()
		{
			for (int i = 0; i < serpentDataList.Count; i++)
			{
				if(serpentDataList[i] == true) continue;
				else
				{
					serpentDataList[i] = true;
					return;
				}
			}
		}

		private GameObject FetchSegmentToSpawn()
		{
			for (int i = 0; i < serpentDataList.Count; i++)
			{
				if(serpentDataList[i] == false)
				{
					nextSegmentToUnlock = segments[i];
					return nextSegmentToUnlock;
				}
			}

			Debug.LogError("Couldn't find next segment to unlock.");
			return null;
		}

		private List<bool> FetchSerpentDataList()
		{
			return serpentDataList;
		}

		public void FixSerpentDelegateLinks()
		{
			serpSplineHandler = FindObjectOfType<SerpentScreenSplineHandler>();
			if (serpSplineHandler != null)
				serpSplineHandler.onFetchSerpDataList += FetchSerpentDataList;

			serpSegHandlers = FindObjectsOfType<SerpentSegmentHandler>();
			foreach (SerpentSegmentHandler handler in serpSegHandlers)
			{
				if (handler != null) handler.onFetchSerpDataList += FetchSerpentDataList;	
			}			
		}

		public void FixGameplayDelegateLinks()
		{
			serpSegHandlers = FindObjectsOfType<SerpentSegmentHandler>();
			foreach (SerpentSegmentHandler handler in serpSegHandlers)
			{
				if (handler != null) handler.onFetchSerpDataList += FetchSerpentDataList;
			}

			segSpawner = FindObjectOfType<SegmentSpawner>();
			if (segSpawner != null) segSpawner.onFetchSegmentToSpawn += FetchSegmentToSpawn;
		}

		private void OnDisable()
		{
			foreach (SerpentSegmentHandler handler in serpSegHandlers)
			{
				if (handler != null) handler.onFetchSerpDataList -= FetchSerpentDataList;
			}

			if (serpSplineHandler != null)
				serpSplineHandler.onFetchSerpDataList -= FetchSerpentDataList;

			if (segSpawner != null) segSpawner.onFetchSegmentToSpawn -= FetchSegmentToSpawn;
		}
	}
}
