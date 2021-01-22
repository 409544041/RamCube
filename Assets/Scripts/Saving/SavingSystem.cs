using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Qbism.WorldMap;

namespace Qbism.Saving
{
	public static class SavingSystem
	{
		public static void SaveProgHandlerData(ProgressHandler progHandler)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			string path = Application.persistentDataPath + "/progression.sav";
			FileStream stream = new FileStream(path, FileMode.Create);

			ProgHandlerData data = new ProgHandlerData(progHandler);

			formatter.Serialize(stream, data);
			stream.Close();
		}

		public static ProgHandlerData LoadProgHandlerData()
		{
			string path = Application.persistentDataPath + "/progression.sav";
			if (File.Exists(path))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(path, FileMode.Open);

				ProgHandlerData data = formatter.Deserialize(stream) as ProgHandlerData;
				stream.Close();

				return data;
			}
			else
			{
				Debug.LogError("Progress Handler save file not found in " + path);
				return null;
			}
		}
	}
}

