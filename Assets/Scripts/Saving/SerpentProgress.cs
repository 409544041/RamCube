using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.Saving
{
	public class SerpentProgress : MonoBehaviour
	{
		//States
		public List<SerpentStatusData> serpentDataList { get; private set; } = new List<SerpentStatusData>();

		private void Awake() 
		{
			BuildSerpentList();
			LoadSerpentData();
		}

		private void BuildSerpentList()
		{
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				SerpentStatusData newData = new SerpentStatusData();
				serpentDataList.Add(newData);
			}
		}

		public void LoadSerpentData()
		{
			ProgData data = SavingSystem.LoadProgData();
			if (data == null) return;

			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				E_SegmentsGameplayData.GetEntity(i).f_Rescued = 
					data.savedSerpentDataList[i].rescued;
				E_SegmentsGameplayData.GetEntity(i).f_AddedToSerpScreen =
					data.savedSerpentDataList[i].addedToSerpScreen;
			}
		}

		public void SaveSerpentData()
		{
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				var serpEntity = E_SegmentsGameplayData.GetEntity(i);
				var serpData = serpentDataList[i];

				serpData.rescued = serpEntity.f_Rescued;
				serpData.addedToSerpScreen = serpEntity.f_AddedToSerpScreen;
			}
		}

		public void WipeSerpentData()
		{
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				E_SegmentsGameplayData.GetEntity(i).f_Rescued = false;
				E_SegmentsGameplayData.GetEntity(i).f_AddedToSerpScreen = false;
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
