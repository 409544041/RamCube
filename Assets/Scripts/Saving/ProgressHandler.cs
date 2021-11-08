using System;
using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using Qbism.WorldMap;
using UnityEngine;

namespace Qbism.Saving
{
	public class ProgressHandler : MonoBehaviour
	{
		//Cache
		SerpentProgress serpProg = null;
		PinSelectionTracker pinSelTrack = null;
		LevelPinUI[] pinUIs = null;
		PinChecker pinChecker = null;
		
		//States
		public E_Pin currentPin { get; set; }
		public E_Biome currentBiome { get; set ; }
		public bool currentHasSegment { get ; set ; }

		List<LevelStatusData> levelDataList = new List<LevelStatusData>();
		public List<bool> biomeDataList = new List<bool>();
		List<LevelPinPathHandler> pinPathers = new List<LevelPinPathHandler>();
		List<LineDrawer> lineDrawers = new List<LineDrawer>();


		private void Awake() 
		{
			serpProg = GetComponent<SerpentProgress>();
			currentPin = E_LevelData.GetEntity(0).f_Pin;
			BuildDataLists();
			LoadProgHandlerData();
		}

		private void BuildDataLists()
		{
			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				LevelStatusData newData = new LevelStatusData();
				levelDataList.Add(newData);
			}

			for (int i = 0; i < E_BiomeGameplayData.CountEntities; i++)
			{
				biomeDataList.Add(false);
			}
		}

		public void FixMapUILinks()
		{
			pinSelTrack = FindObjectOfType<PinSelectionTracker>();
			if (pinSelTrack != null) pinSelTrack.onSavedPinFetch += FetchCurrentPin;

			pinUIs = FindObjectsOfType<LevelPinUI>();
			foreach (LevelPinUI pinUI in pinUIs)
			{
				if (pinUI != null)
					pinUI.onSetCurrentData += SetCurrentData;
			}
		}

		public void FixMapPinLinks()
		{
			pinChecker = FindObjectOfType<PinChecker>();

			pinPathers.Clear();
			lineDrawers.Clear();

			for (int i = 0; i < pinChecker.levelPins.Length; i++)
			{
				pinPathers.Add(pinChecker.levelPins[i].pinPather);

				LineDrawer[] drawers = pinChecker.levelPins[i].GetComponentsInChildren<LineDrawer>();
				for (int j = 0; j < drawers.Length; j++)
				{
					lineDrawers.Add(drawers[j]);
				}

				if (pinPathers[i] != null)
					pinPathers[i].onGetPin += FetchPin;
			}

			for (int i = 0; i < lineDrawers.Count; i++)
			{
				if (lineDrawers[i] != null)
				{
					lineDrawers[i].onSaveData += SaveProgData;
					lineDrawers[i].onDisableLockInSheet += SetLockDisabledValue;
				}
			}
		}

		public void SetLevelToComplete(E_Pin pin)
		{
			var entity = E_LevelGameplayData.FindEntity(entity =>
			entity.f_Pin == pin);

			if (pin == null)
				Debug.LogError("Couldn't find correct entity");

			if (entity.f_Completed == false) 
			{
				entity.f_Completed = true;
				CheckLevelsToUnlock(pin);
			}
		}

		private void CheckLevelsToUnlock(E_Pin pin)
		{
			var levelEntity = E_LevelData.FindEntity(entity =>
				entity.f_Pin == pin);

			for (int j = 0; j < levelEntity.f_UnlocksPins.Count; j++)
			{
				SetUnlockedStatus(levelEntity.f_UnlocksPins[j], true);
			}
		}

		public void SetUnlockedStatus(E_Pin pin, bool value)
		{
			var entity = E_LevelGameplayData.FindEntity(entity =>
			entity.f_Pin == pin);

			if (entity.f_LocksLeft > 0) entity.f_LocksLeft--;
			if (entity.f_LocksLeft == 0) entity.f_Unlocked = value;

		}

		public LevelPin[] FetchLevelPins()
		{
			return pinChecker.levelPins;
		}

		private void SetCurrentData(E_Pin pin, bool hasSegment, E_Biome biome)
		{
			currentPin = pin;
			currentBiome = biome;

			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				var entity = E_LevelGameplayData.GetEntity(i);
				if (entity.f_Pin == pin)
				{
					if (entity.f_Completed) currentHasSegment = false;
					else currentHasSegment = hasSegment;
				}
			}
		}

		private LevelPin FetchPin(E_Pin pin)
		{
			LevelPin foundPin = null;

			for (int i = 0; i < pinChecker.levelPins.Length; i++)
			{
				if (pinChecker.levelPins[i].m_levelData.f_Pin == pin)
				return pinChecker.levelPins[i];
			}
			Debug.LogError("Couldn't find correct levelPin");
			return foundPin;
		}

		private void FetchCurrentPin()
		{
			for (int i = 0; i < pinChecker.levelPins.Length; i++)
			{
				if (pinChecker.levelPins[i].m_Pin.Entity != currentPin) continue;

				pinSelTrack.selectedPin = pinChecker.levelPins[i];
				pinSelTrack.currentBiome = pinChecker.levelPins[i].m_Pin.f_Biome;
			}
		}

		private void SetLockDisabledValue(E_LevelGameplayData entity ,bool value)
		{
			entity.f_LockIconDisabled = value;
		}

		public void SaveProgData()
		{
			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				var gameplayEntity = E_LevelGameplayData.GetEntity(i);
				var levelData = levelDataList[i];

				levelData.pin = gameplayEntity.f_Pin.f_name.ToString(); 
				levelData.locksLeft = gameplayEntity.f_LocksLeft;
				levelData.dottedAnimPlayed = gameplayEntity.f_DottedAnimPlayed;
				levelData.unlocked = gameplayEntity.f_Unlocked; 
				levelData.completed = gameplayEntity.f_Completed;
				levelData.unlockAnimPlayed = gameplayEntity.f_UnlockAnimPlayed;
				levelData.pathDrawn = gameplayEntity.f_PathDrawn;
				levelData.lockDisabled = gameplayEntity.f_LockIconDisabled;
				levelData.wallDown = gameplayEntity.f_wallDown;
			}

			for (int i = 0; i < E_BiomeGameplayData.CountEntities; i++)
			{
				biomeDataList[i] = E_BiomeGameplayData.GetEntity(i).f_Unlocked;
			}

			SavingSystem.SaveProgData(levelDataList, biomeDataList, currentPin.f_name.ToString(),
				serpProg.serpentDataList);
		}

		public void LoadProgHandlerData()
		{
			ProgData data = SavingSystem.LoadProgData();

			levelDataList = data.savedLevelData;
			biomeDataList = data.savedBiomeData;
			
			string currentPinString = data.savedCurrentPin;
			currentPin = E_Pin.FindEntity(entity =>
					entity.f_name == currentPinString);

			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				var entity = E_LevelGameplayData.GetEntity(i);
				var levelData = levelDataList[i];
				E_Pin pin = E_LevelData.FindEntity(entity =>
					entity.f_Pin.f_name.ToString() == levelData.pin).f_Pin;
				entity.f_Pin = pin;
				entity.f_LocksLeft = levelData.locksLeft;
				entity.f_DottedAnimPlayed = levelData.dottedAnimPlayed;
				entity.f_Unlocked = levelData.unlocked; entity.f_Completed = levelData.completed;
				entity.f_UnlockAnimPlayed = levelData.unlockAnimPlayed;
				entity.f_PathDrawn = levelData.pathDrawn;
				entity.f_LockIconDisabled = levelData.lockDisabled;
				entity.f_wallDown = levelData.wallDown;
			}

			for (int i = 0; i < E_BiomeGameplayData.CountEntities; i++)
			{
				E_BiomeGameplayData.GetEntity(i).f_Unlocked = biomeDataList[i];
			}
		}

		public void WipeProgData() //TO DO: Make this debug only
		{
			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				var levelData = E_LevelGameplayData.GetEntity(i);

				levelData.f_LocksLeft = E_LevelData.GetEntity(i).f_LocksAmount;
				levelData.f_DottedAnimPlayed = false; levelData.f_Completed = false;
				levelData.f_PathDrawn = false;
				levelData.f_LockIconDisabled = false;
				levelData.f_wallDown = false;

				if (i == 0)
				{
					levelData.f_Unlocked = true;
					levelData.f_UnlockAnimPlayed = true;
				}
				else
				{
					levelData.f_Unlocked = false;
					levelData.f_UnlockAnimPlayed = false;
				}			
			}

			for (int i = 0; i < E_BiomeGameplayData.CountEntities; i++)
			{
				var biomeGameplayData = E_BiomeGameplayData.GetEntity(i);
				if (i == 0) biomeGameplayData.f_Unlocked = true;
				else biomeGameplayData.f_Unlocked = false;
			}

			currentPin = E_LevelData.GetEntity(0).f_Pin;

			for (int i = 0; i < serpProg.serpentDataList.Count; i++)
			{
				serpProg.serpentDataList[i] = false;
			}

			SaveProgData();
		}

		private void OnDisable()
		{
			if (pinSelTrack != null) pinSelTrack.onSavedPinFetch -= FetchCurrentPin;

			foreach (LevelPinUI pinUI in pinUIs)
			{
				if (pinUI != null)
					pinUI.onSetCurrentData -= SetCurrentData;
			}

			for (int i = 0; i < pinPathers.Count; i++)
			{
				if (pinPathers[i] != null)
					pinPathers[i].onGetPin -= FetchPin;
			}

			for (int i = 0; i < lineDrawers.Count; i++)
			{
				if (lineDrawers[i] != null)
				{
					lineDrawers[i].onSaveData -= SaveProgData;
					lineDrawers[i].onDisableLockInSheet -= SetLockDisabledValue;
				}
			}
		}
	}
}
