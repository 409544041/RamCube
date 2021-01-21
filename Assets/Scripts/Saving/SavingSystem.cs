using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Qbism.Saving
{
	public static class SavingSystem
	{
		public static void SaveProgression(ProgressHandler progHandler)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			string path = Application.persistentDataPath + "/progression.sav";
			FileStream stream = new FileStream(path, FileMode.Create);

			ProgressData data = new ProgressData(progHandler);

			formatter.Serialize(stream, data);
			stream.Close();
		}

		public static ProgressData LoadProgression()
		{
			string path = Application.persistentDataPath + "/progression.sav";
			if (File.Exists(path))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(path, FileMode.Open);

				ProgressData data = formatter.Deserialize(stream) as ProgressData;
				stream.Close();

				return data;
			}
			else
			{
				Debug.LogError("Save file not found in " + path);
				return null;
			}
		}
	}
}

