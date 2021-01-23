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
		[SerializeField] float lockedYPos;
		[SerializeField] float unlockedYPos;
		[SerializeField] float raiseStep;
		[SerializeField] float raiseSpeed;

		//Cache
		ProgressHandler progHandler;

		//States
		public bool unlocked { get; set; }
		public bool unlockAnimPlayed { get; set; }
		public bool completed { get; set; }
		bool raising;

		private void Start()
		{
			progHandler = FindObjectOfType<ProgressHandler>();

			CheckUnlockStatus();
			CheckCompleteStatus();
		}

		private void CheckCompleteStatus()
		{
			if(levelID == LevelIDs.a_01) return;

			foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)
				if(data.levelID == levelID) completed = data.completed;
		}

		private void CheckUnlockStatus()
		{
			if(levelID == LevelIDs.a_01) return;

			foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)
			{
				if (data.levelID == levelID)
				{
					unlockAnimPlayed = data.unlockAnimPlayed;
					unlocked = data.unlocked;
				} 
			}
			GetComponent<ClickableObject>().canClick = unlocked;
			
			if(unlocked && !unlockAnimPlayed)
			{
				foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)
					if (data.levelID == levelID) data.unlockAnimPlayed = true;

				unlockAnimPlayed = true;				
				progHandler.SaveProgHandlerData();
				
				raising = true;
				StartCoroutine(RaiseCliff());
			}
			else if(unlocked && unlockAnimPlayed)
			{
				transform.position = new Vector3(
					transform.position.x, unlockedYPos, transform.position.z);
			}
		}

		private IEnumerator RaiseCliff()
		{
			GetComponent<LevelPinRaiseJuicer>().PlayRaiseJuice();

			while(raising)
			{
				transform.position += new Vector3 (0, raiseStep, 0);

				yield return new WaitForSeconds(raiseSpeed);

				if (transform.position.y >= unlockedYPos)
				{
					raising = false;
					transform.position = new Vector3(
						transform.position.x, unlockedYPos, transform.position.z);

					GetComponent<LevelPinRaiseJuicer>().StopRaiseJuice();
				}
			}
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
