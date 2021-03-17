using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class SerpentProgress : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int serpentLength = 20;

		//States
		public List<bool> serpentDataList;

		private void Awake() 
		{
			BuildSerpentList();
			LoadSerpentData();
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
	}
}
