using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;
using BansheeGz.BGDatabase;

[System.Serializable]
public class ProgData
{
	//States
	public List<LevelStatusData> savedLevelData;
	public List<bool> savedBiomeData;
	public string savedCurrentPin;
	public List<SerpentStatusData> savedSerpentDataList;
	public List<ObjectStatusData> savedObjectsData;

	public ProgData(List<LevelStatusData> levelData, List<bool> biomeData,
		string currentPin, List<SerpentStatusData> serpentDataList, List<ObjectStatusData> objectsData)
	{
		savedLevelData = levelData;
		savedBiomeData = biomeData;
		savedCurrentPin = currentPin;
		savedSerpentDataList = serpentDataList;
		savedObjectsData = objectsData;
	}
}
