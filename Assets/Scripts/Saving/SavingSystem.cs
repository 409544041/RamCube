﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Qbism.Saving
{
	// save location = C:\Users\js_ba\AppData\LocalLow\Frambosa\Billy Bumbum
	public static class SavingSystem
	{
		//States
		public static string saveName;

		public static void SaveProgData(List<LevelStatusData> levelDataList, List<bool> biomeDataList,
			string currentPin, List<SerpentStatusData> serpentDataList, List<ObjectStatusData> objectsDataList,
			SettingsValueData settingsData)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			string path = Application.persistentDataPath + saveName;
			FileStream stream = new FileStream(path, FileMode.Create);

			ProgData data = new ProgData(levelDataList, biomeDataList, currentPin, 
				serpentDataList, objectsDataList, settingsData);

			formatter.Serialize(stream, data);
			stream.Close();
		}

		public static ProgData LoadProgData()
		{
			string path = Application.persistentDataPath + saveName;
			if (File.Exists(path))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(path, FileMode.Open);

				ProgData data = formatter.Deserialize(stream) as ProgData;
				stream.Close();

				return data;
			}
			else
			{
				Debug.Log("Progress Handler save file not found in " + path);
				return null;
			}
		}
	}
}

