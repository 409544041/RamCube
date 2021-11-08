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
	public List<bool> savedSerpentDataList;

	public ProgData(List<LevelStatusData> levelData, List<bool> biomeData,
		string currentPin, List<bool> serpentDataList)
	{
		savedLevelData = levelData;
		savedBiomeData = biomeData;
		savedCurrentPin = currentPin;
		savedSerpentDataList = serpentDataList;
	}
}
