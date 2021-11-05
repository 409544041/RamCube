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
		
		//States
		public E_Pin currentPin { get; set; }
		public E_Biome currentBiome { get; set ; }
		public bool currentHasSegment { get ; set ; }

		List<LevelStatusData> levelDataList = new List<LevelStatusData>();
		public List<LevelPin> levelPinList { get; set; } = new List<LevelPin>();
		List<LevelPinPathHandler> pinPathers = new List<LevelPinPathHandler>();
		List<LineDrawer> lineDrawers = new List<LineDrawer>();


		private void Awake() 
		{
			serpProg = GetComponent<SerpentProgress>();
			currentPin = E_LevelData.GetEntity(0).f_Pin;
			BuildLevelDataList();
			LoadProgHandlerData();
		}

		private void BuildLevelDataList()
		{
			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				LevelStatusData newData = new LevelStatusData();
				levelDataList.Add(newData);
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
			pinPathers.Clear();
			lineDrawers.Clear();

			for (int i = 0; i < levelPinList.Count; i++)
			{
				pinPathers.Add(levelPinList[i].pinPather);

				LineDrawer[] drawers = levelPinList[i].GetComponentsInChildren<LineDrawer>();
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

		public void BuildLevelPinList()
		{
			levelPinList.Clear();

			foreach (LevelPin pin in FindObjectsOfType<LevelPin>())
			{
				levelPinList.Add(pin);
			}

			levelPinList.Sort(PinsByID);
		}

		private int PinsByID(LevelPin A, LevelPin B)
		{
			if(A.m_Pin.f_Index < B.m_Pin.f_Index) return -1;
			else if(A.m_Pin.f_Index > B.m_Pin.f_Index) return 1;
			return 0;
		}

		public void HandleLevelPins()
		{
			for (int i = 0; i < levelPinList.Count; i++)
			{
				var pin = levelPinList[i];
				List<LevelPin> originPins = new List<LevelPin>();
				AddOriginPins(pin, originPins);

				var levelEntity = E_LevelData.FindEntity(entity => 
					entity.f_Pin == pin.m_levelData.f_Pin);
				var gameplayEntity = E_LevelGameplayData.FindEntity(entity => 
					entity.f_Pin == pin.m_levelData.f_Pin);

				int locksAmount, locksLeft; List<E_MapWalls> originWalls;
				bool dottedAnimPlayed, unlockAnimPlayed, unlocked, completed, pathDrawn, wallDown;

				FetchPinValues(levelEntity, gameplayEntity, out locksAmount, out locksLeft, out dottedAnimPlayed, 
					out unlockAnimPlayed, out unlocked, out completed, out pathDrawn, out originWalls, out wallDown);

				pin.justCompleted = false;
				if (completed && !pathDrawn) pin.justCompleted = true;

				bool uUnlocked, uUnlockAnimPlayed; int uLocksLeft; List<E_MapWalls> uOriginWalls;
				bool checkedRaiseStatus = false;
				List<E_Pin> unlockPins = levelEntity.f_UnlocksPins;

				for (int j = 0; j < unlockPins.Count; j++)
				{
					FetchUnlockData(unlockPins[j], out uUnlocked, out uUnlockAnimPlayed,
						out uLocksLeft, out uOriginWalls);

					if (!checkedRaiseStatus) pin.CheckRaiseStatus(unlocked, unlockAnimPlayed);
					checkedRaiseStatus = true;

					pin.pinPather.CheckPathStatus(unlockPins[j], uUnlocked, uUnlockAnimPlayed, uLocksLeft, 
						completed, unlockPins.Count, uOriginWalls, wallDown);
				}
			
				SetPinUI(pin, unlockAnimPlayed, completed);
				StartCoroutine(RaiseAndDrawPaths(gameplayEntity, pin, originPins, locksAmount, locksLeft, dottedAnimPlayed, 
					unlockAnimPlayed, unlocked, completed, pathDrawn, originWalls));
			}

			SaveProgData();
		}

		private void AddOriginPins(LevelPin incomingPin, List<LevelPin> originPins)
		{
			for (int i = 0; i < levelPinList.Count; i++)
			{
				var unlockPins = levelPinList[i].m_levelData.f_UnlocksPins;

				if (unlockPins != null)
				{
					for (int j = 0; j < unlockPins.Count; j++)
					{
						if (unlockPins[j] == incomingPin.m_levelData.f_Pin)
							originPins.Add(levelPinList[i]);									
					}
				}
			}
		}

		private void FetchPinValues(E_LevelData levelEntity, E_LevelGameplayData gameplayEntity, out int locksAmount,
			out int locksLeft, out bool dottedAnimPlayed, out bool unlockAnimPlayed, out bool unlocked, 
			out bool completed, out bool pathDrawn, out List<E_MapWalls> originWallList, out bool wallDown)
		{
			locksAmount = levelEntity.f_LocksAmount;
			locksLeft = gameplayEntity.f_LocksLeft;
			dottedAnimPlayed = gameplayEntity.f_DottedAnimPlayed;
			unlockAnimPlayed = gameplayEntity.f_UnlockAnimPlayed;
			unlocked = gameplayEntity.f_Unlocked;
			completed = gameplayEntity.f_Completed;
			pathDrawn = gameplayEntity.f_PathDrawn;
			originWallList = levelEntity.f_WallsFromOrigin;
			wallDown = gameplayEntity.f_wallDown;
		}

		private void FetchUnlockData(E_Pin unlockPin, out bool uUnlocked,
			out bool uUnlockAnimPlayed, out int uLocksLeft, out List<E_MapWalls> uOriginWalls)
		{
			var entity = E_LevelGameplayData.FindEntity(entity =>
				entity.f_Pin == unlockPin);
			var levelEntity = E_LevelData.FindEntity(entity =>
				entity.f_Pin == unlockPin);
			
			uUnlocked = entity.f_Unlocked; uUnlockAnimPlayed = entity.f_UnlockAnimPlayed;
			uLocksLeft = entity.f_LocksLeft; uOriginWalls = levelEntity.f_WallsFromOrigin;
		}

		private void SetPinUI(LevelPin pin, bool unlockAnimPlayed, bool completed)
		{
			pin.pinUI.SelectPinUI(); //Why this? Remove?

			if (unlockAnimPlayed) pin.pinUI.ShowOrHideUI(true);
			else pin.pinUI.ShowOrHideUI(false);

			if (completed) pin.pinUI.SetUIComplete();

			pin.pinUI.DisableLockIcon();
		}

		private IEnumerator RaiseAndDrawPaths(E_LevelGameplayData entity, LevelPin pin, 
			List<LevelPin> originPins, int locksAmount, int locksLeft, bool dottedAnimPlayed, 
			bool unlockAnimPlayed, bool unlocked, bool completed, bool pathDrawn, List<E_MapWalls> originWalls)
		{
			bool lessLocks = (locksAmount > locksLeft) && locksLeft != 0;
			bool raised = false;

			var wallsFromOrigin = 0;
			if (originWalls != null) wallsFromOrigin = originWalls.Count;
			int loweredWalls = 0;

			for (int i = 0; i < originPins.Count; i++)
			{
				var originPin = originPins[i];
				var linkedWall = false;

				if (originWalls != null)
				{
					for (int j = 0; j < originWalls.Count; j++)
					{
						if (originPin.m_levelData.f_Pin == originWalls[j].f_OriginPin)
							linkedWall = true;
					}
				}

				bool originDottedAnimPlayed = E_LevelGameplayData.FindEntity(entity =>
						entity.f_Pin == originPin.m_levelData.f_Pin).f_DottedAnimPlayed;

				if (lessLocks)
				{
					//for pins with still one or more locks left
					if (!originDottedAnimPlayed && originPin.justCompleted)
					{
						if (linkedWall)
							originPin.pinPather.DrawToGate(LineTypes.dotted, pin.pinPather.pathPoint, false);
						
						else originPin.pinPather.DrawNewPath(LineTypes.dotted, pin.pinPather.pathPoint, false);

						originDottedAnimPlayed = true;
					}
				}

				if (unlocked && !unlockAnimPlayed)
				{
					//for newly unlocked pins that need to be raised
					if (locksLeft == 0)
					{
						//if !biome same then first lower gates of origins. Afterwards raise pin
						if (linkedWall)
						{							
							yield return originPin.wallLowerer.InitiateWallLowering();
							loweredWalls++;
						}

						//the !raised is bc this is not originPin specific and only needs to happen once
						if (!raised && loweredWalls == wallsFromOrigin)
						{
							pin.pinRaiser.InitiateRaising(originPins, originDottedAnimPlayed);
							entity.f_UnlockAnimPlayed = true;
							raised = true;
						}
					}
				}
				// for pins that have already been unlocked by another level and a second path is 
				// now coming towards it
				else if (unlocked && unlockAnimPlayed && originPin.justCompleted) 
					originPin.pinPather.DrawNewPath(LineTypes.full, pin.pinPather.pathPoint, false);
			}

			if (completed && !pathDrawn) entity.f_PathDrawn = true;
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

		private List<LevelPin> FetchLevelPinList()
		{
			return levelPinList;
		}

		private LevelPin FetchPin(E_Pin pin)
		{
			LevelPin foundPin = null;

			for (int i = 0; i < levelPinList.Count; i++)
			{
				if (levelPinList[i].m_levelData.f_Pin == pin)
				return levelPinList[i];
			}
			Debug.LogError("Couldn't find correct levelPin");
			return foundPin;
		}

		private void FetchCurrentPin()
		{
			for (int i = 0; i < levelPinList.Count; i++)
			{
				if (levelPinList[i].m_Pin.Entity != currentPin) continue;

				pinSelTrack.selectedPin = levelPinList[i];
				pinSelTrack.currentBiome = levelPinList[i].m_Pin.f_Biome;
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
				var entity = E_LevelGameplayData.GetEntity(i);
				var levelData = levelDataList[i];

				levelData.pin = entity.f_Pin.f_name.ToString(); levelData.locksLeft = entity.f_LocksLeft;
				levelData.dottedAnimPlayed = entity.f_DottedAnimPlayed;
				levelData.unlocked = entity.f_Unlocked; levelData.completed = entity.f_Completed;
				levelData.unlockAnimPlayed = entity.f_UnlockAnimPlayed;
				levelData.pathDrawn = entity.f_PathDrawn;
				levelData.lockDisabled = entity.f_LockIconDisabled;
			}

			SavingSystem.SaveProgData(levelDataList, currentPin.f_name.ToString(),
				serpProg.serpentDataList);
		}

		public void LoadProgHandlerData()
		{
			ProgData data = SavingSystem.LoadProgData();

			levelDataList = data.savedLevelData;
			
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
