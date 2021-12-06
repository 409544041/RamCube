using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class PinChecker : MonoBehaviour
	{
		//Config parameters
		public LevelPin[] levelPins = null;
		[SerializeField] PinHandler pinHandler = null;
		[SerializeField] MapDebugCompleter debugComp;

		//Cache
		ProgressHandler progHandler = null;

		private void Awake() 
		{
			progHandler = FindObjectOfType<ProgressHandler>();
		}

		private void Start() 
		{
			debugComp.CheckDebugStatuses();
			CheckLevelPins();
		}

		public void CheckLevelPins()
		{
			for (int i = 0; i < levelPins.Length; i++)
			{
				var pin = levelPins[i];
				List<LevelPin> originPins = new List<LevelPin>();
				AddOriginPins(pin, originPins);

				var levelEntity = E_LevelData.FindEntity(entity =>
					entity.f_Pin == pin.m_levelData.f_Pin);
				var gameplayEntity = E_LevelGameplayData.FindEntity(entity =>
					entity.f_Pin == pin.m_levelData.f_Pin);
				var biomeGameplayEntity = E_BiomeGameplayData.FindEntity(entity =>
					entity.f_Biome == pin.m_levelData.f_Pin.f_Biome);

				int locksAmount, locksLeft; List<E_MapWalls> originWalls;
				bool dottedAnimPlayed, unlockAnimPlayed, unlocked, completed, pathDrawn, wallDown;
				bool biomeUnlocked = biomeGameplayEntity.f_Unlocked;

				FetchPinValues(levelEntity, gameplayEntity, out locksAmount, out locksLeft,
					out dottedAnimPlayed, out unlockAnimPlayed, out unlocked, out completed, out pathDrawn,
					out originWalls, out wallDown);

				pin.justCompleted = false;
				if (completed && !pathDrawn) pin.justCompleted = true;

				bool uUnlocked = false, uUnlockAnimPlayed = false; int uLocksLeft = 2; 
				List<E_MapWalls> uOriginWalls = null; 
				
				bool checkedRaiseAndWallStatus = false;
				List<E_Pin> unlockPins = levelEntity.f_UnlocksPins;

				for (int j = 0; j < unlockPins.Count; j++)
				{
					if (unlockPins[j].f_name != "_EMPTY")
						FetchUnlockData(unlockPins[j], out uUnlocked, out uUnlockAnimPlayed,
							out uLocksLeft, out uOriginWalls);

					if (!checkedRaiseAndWallStatus)
					{
						pin.pinRaiser.CheckRaiseStatus(unlocked, unlockAnimPlayed, biomeUnlocked);
						if (pin.wallLowerer.hasWall) pin.wallLowerer.CheckWallStatus(wallDown);
						checkedRaiseAndWallStatus = true;
					}

					if (unlockPins[j].f_name != "_EMPTY")
						pin.pinPather.CheckPathStatus(unlockPins[j], uUnlocked, uUnlockAnimPlayed, uLocksLeft,
							completed, unlockPins.Count, uOriginWalls, wallDown, dottedAnimPlayed);
				}

				pinHandler.SetPinUI(pin, unlockAnimPlayed, completed, pin.justCompleted);
				pinHandler.InitiateRaiseAndDrawPaths(gameplayEntity, pin, originPins, locksAmount, locksLeft,
					dottedAnimPlayed, unlockAnimPlayed, unlocked, completed, pathDrawn, originWalls,
					biomeUnlocked);
			}

			progHandler.SaveProgData();
		}

		private void AddOriginPins(LevelPin incomingPin, List<LevelPin> originPins)
		{
			for (int i = 0; i < levelPins.Length; i++)
			{
				var unlockPins = levelPins[i].m_levelData.f_UnlocksPins;

				if (unlockPins != null)
				{
					for (int j = 0; j < unlockPins.Count; j++)
					{
						if (unlockPins[j] == incomingPin.m_levelData.f_Pin)
							originPins.Add(levelPins[i]);
					}
				}
			}
		}

		private void FetchPinValues(E_LevelData levelEntity, E_LevelGameplayData gameplayEntity,
			out int locksAmount, out int locksLeft, out bool dottedAnimPlayed, out bool unlockAnimPlayed,
			out bool unlocked, out bool completed, out bool pathDrawn, out List<E_MapWalls> originWallList,
			out bool wallDown)
		{
			locksAmount = levelEntity.f_LocksAmount;
			locksLeft = gameplayEntity.f_LocksLeft;
			dottedAnimPlayed = gameplayEntity.f_DottedAnimPlayed;
			unlockAnimPlayed = gameplayEntity.f_UnlockAnimPlayed;
			unlocked = gameplayEntity.f_Unlocked;
			completed = gameplayEntity.f_Completed;
			pathDrawn = gameplayEntity.f_PathDrawn;
			originWallList = levelEntity.f_WallsFromOrigin;
			wallDown = gameplayEntity.f_WallDown;
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
	}

}