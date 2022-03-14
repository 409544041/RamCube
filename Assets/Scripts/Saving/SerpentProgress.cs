using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.Saving
{
	public class SerpentProgress : MonoBehaviour
	{
		//States
		public List<bool> serpentDataList { get; private set; } = new List<bool>();

		private void Awake() 
		{
			BuildSerpentList();
			LoadSerpentData();
		}

		private void BuildSerpentList()
		{
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

		public void WipeSerpentData()
		{
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				E_SegmentsGameplayData.GetEntity(i).f_Rescued = false;
			}
		}

		public void AddSegmentToDatabase()
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
	}
}
