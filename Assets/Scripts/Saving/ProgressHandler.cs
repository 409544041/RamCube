using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class ProgressHandler : MonoBehaviour
	{
		//States
		public LevelIDs currentLevelID { get; set; }
		public bool isCompleted { get; set; }

		public Dictionary<LevelIDs, bool> levelCompleteDic =
				new Dictionary<LevelIDs, bool>();

		public Dictionary<LevelIDs, bool> levelUnlockDic =
				new Dictionary<LevelIDs, bool>();

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.L)) LoadProgress(); //TO DO: Temporary solution just to test. Fix later.
			if (Input.GetKeyDown(KeyCode.P)) WipeProgress();
		}
		public void SetLevelToComplete()
		{
			isCompleted = true;
			levelCompleteDic.Add(currentLevelID, true);
		}

		public void SaveProgress()
		{
			SavingSystem.SaveProgression(this);
		}

		public void LoadProgress()
		{
			ProgressData data = SavingSystem.LoadProgression();

			levelCompleteDic = data.savedLevelComleteDic;
			levelUnlockDic = data.savedLevelUnlockDic;
		}

		public void WipeProgress()
		{
			levelCompleteDic.Clear();
			levelUnlockDic.Clear();
			SavingSystem.SaveProgression(this);
		}
	}
}
