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
		[SerializeField] Material unlockedCliffMat;
		[SerializeField] Material unlockedGroundMat;
		[SerializeField] Material unlockingCliffMat;
		[SerializeField] Material unlockingGroundMat;
		[SerializeField] float unlockStep;
		[SerializeField] float unlockSpeed;

		//Cache
		ProgressHandler progHandler;

		//States
		bool unlocked;
		bool unlockAnimPlayed;
		bool completed;

		private void Start()
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			//progHandler.LoadProgHandlerData();

			ActivateFirstPinValues();
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

			MeshRenderer mesh = GetComponent<MeshRenderer>();
			Material[] mats = mesh.materials;

			foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)

				if (data.levelID == levelID)
				{
					unlockAnimPlayed = data.unlockAnimPlayed;
					unlocked = data.unlocked;
				} 
			GetComponent<ClickableObject>().canClick = unlocked;
			
			if(unlocked && !unlockAnimPlayed)
			{
				foreach (ProgressHandler.LevelStatusData data in progHandler.levelDataList)
					if (data.levelID == levelID) data.unlockAnimPlayed = true;
				
				progHandler.SaveProgHandlerData();
				HandleUnlockingMats(mesh, mats);
			}
			else if(unlocked && unlockAnimPlayed)
			{
				SetToUnlockedMat(mesh, mats);
			}
		}

		private void HandleUnlockingMats(MeshRenderer mesh, Material[] mats)
		{
			float startHeight = mats[0].GetFloat("_GradientCenterY");

			mats[0] = unlockingCliffMat;
			mats[1] = unlockingGroundMat;
			mesh.materials = mats;

			StartCoroutine(LowerHeightGradient(mesh, mats, startHeight));
		}

		private IEnumerator LowerHeightGradient(MeshRenderer mesh, Material[] mats, float startHeight)
		{
			float gradientBottom = unlockedCliffMat.GetFloat("_GradientCenterY");

			while(mats[0].GetFloat("_GradientCenterY") > gradientBottom && mats[0] == unlockingCliffMat)
			{
				foreach (Material mat in mats)
				{
					float currentHeight = mat.GetFloat("_GradientCenterY");
					mat.SetFloat("_GradientCenterY", currentHeight - unlockStep);
				}
				yield return new WaitForSeconds(unlockSpeed);
			}

			if(mats[0].GetFloat("_GradientCenterY") <= gradientBottom)
			{
				SetToUnlockedMat(mesh, mats);
				ResetHeightGradient(startHeight);
			}
		}

		private void ResetHeightGradient(float startHeight)
		{
			unlockingCliffMat.SetFloat("_GradientCenterY", startHeight);
			unlockingGroundMat.SetFloat("_GradientCenterY", startHeight);
		}

		private void SetToUnlockedMat(MeshRenderer mesh, Material[] mats)
		{
			mats[0] = unlockedCliffMat;
			mats[1] = unlockedGroundMat;
			mesh.materials = mats;
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

		private void ActivateFirstPinValues()
		{
			if (levelID == LevelIDs.a_01)
			{
				unlocked = true;
				unlockAnimPlayed = true;
				completed = true;

				MeshRenderer mesh = GetComponent<MeshRenderer>();
				Material[] mats = mesh.materials;
				mats[0] = unlockedCliffMat;
				mats[1] = unlockedGroundMat;
				mesh.materials = mats;
			}
		}
	}
}
