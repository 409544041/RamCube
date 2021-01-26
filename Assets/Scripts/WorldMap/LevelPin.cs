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
		[SerializeField] Transform pathPoint;

		//Cache
		ProgressHandler progHandler;

		//States
		public bool unlocked { get; set; }
		public bool unlockAnimPlayed { get; set; }
		public bool completed { get; set; }
		bool pathDrawn = false;
		bool justCompleted = false;
		bool raising = false;

		private void Start()
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			if (progHandler != null)
				progHandler.onRaisedCliff += InitiateDrawPath;

			CheckUnlockStatus();
			CheckCompleteStatus();
		}

		private void CheckCompleteStatus()
		{
			justCompleted = false;

			foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)
				if (data.levelID == levelID)
				{
					completed = data.completed;
					pathDrawn = data.pathDrawn;
				} 

			if(completed && !pathDrawn)
			{
				foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)
					if (data.levelID == levelID) data.pathDrawn = true;

				pathDrawn = true;
				justCompleted = true;
			}
		}

		private void CheckUnlockStatus()
		{
			if (levelID == LevelIDs.a_01) return;
			FetchStatusFromProgHandler();
			CheckRaiseStatus();
		}

		private void FetchStatusFromProgHandler()
		{
			foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)
			{
				if (data.levelID == levelID)
				{
					unlockAnimPlayed = data.unlockAnimPlayed;
					unlocked = data.unlocked;
				}
			}
			GetComponent<ClickableObject>().canClick = unlocked;
		}

		private void CheckRaiseStatus()
		{
			if (unlocked && !unlockAnimPlayed)
			{
				foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)
					if (data.levelID == levelID) data.unlockAnimPlayed = true;

				unlockAnimPlayed = true;
				progHandler.SaveProgHandlerData();

				raising = true;
				StartCoroutine(RaiseCliff());
			}
			else if (unlocked && unlockAnimPlayed)
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
					progHandler.StartDrawingPath(pathPoint);
				}
			}
		}

		private void InitiateDrawPath(Transform point)
		{
			if(!justCompleted) return;

			LineDrawer drawer = GetComponentInChildren<LineDrawer>();
			drawer.SetPositions(pathPoint.transform, point.transform);

			pathPoint.GetComponentInChildren<LineRenderer>().enabled = true;
			drawer.drawing = true;
		}

		public void LoadAssignedLevel() //Called from Unity Event on Clickable Object
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

		private void OnDisable()
		{
			if (progHandler != null)
				progHandler.onRaisedCliff -= InitiateDrawPath;
		}
	}
}
