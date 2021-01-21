using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPin : MonoBehaviour
	{
		//Config parameters
		public LevelIDs levelID;

		//Cache
		ProgressHandler progHandler;

		//States
		public bool unlocked { get; set; }
		public bool completed { get; set; }

		private void Awake() 
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			progHandler.LoadProgress();
		}

		private void Start() 
		{
			if(progHandler.levelCompleteDic.ContainsKey(levelID))
				completed = progHandler.levelCompleteDic[levelID];
			else completed = false;
		}

		public void LoadAssignedLevel()
		{
			SetCurrentLevelID();
			var handler = FindObjectOfType<SceneHandler>();
			int indexToLoad = GetComponent<EditorSetPinValues>().levelIndex;
			handler.LoadBySceneIndex(indexToLoad);
		}

		private void SetCurrentLevelID()
		{
			FindObjectOfType<ProgressHandler>().currentLevelID = levelID;
		}
	}
}
