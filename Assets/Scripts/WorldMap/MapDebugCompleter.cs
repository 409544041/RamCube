using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class MapDebugCompleter : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MapCoreRefHolder mcRef;

		//States
		PersistentRefHolder persRef;

		private void Awake()
		{
			persRef = mcRef.persRef;
		}

		public void UnCompleteAll()
		{
			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				E_LevelGameplayData.GetEntity(i).f_DebugUncomplete = true;
			}

			for (int i = 0; i < E_BiomeGameplayData.CountEntities; i++)
			{
				if (i == 0) continue;
				E_BiomeGameplayData.GetEntity(i).f_Unlocked = false;
			}
			
			for (int i = 0; i < E_SegmentsGameplayData.CountEntities; i++)
			{
				E_SegmentsGameplayData.GetEntity(i).f_Rescued = false;
				E_SegmentsGameplayData.GetEntity(i).f_AddedToSerpScreen = false;
			}

			for (int i = 0; i < E_ObjectsGameplayData.CountEntities; i++)
			{
				E_ObjectsGameplayData.GetEntity(i).f_ObjectFound = false;
				E_ObjectsGameplayData.GetEntity(i).f_ObjectReturned = false;
			}

			persRef.progHandler.currentPin = E_Pin.GetEntity(0);
		}

		public void CompleteAll()
		{
			for (int i = 0; i < E_BiomeGameplayData.CountEntities; i++)
			{
				E_BiomeGameplayData.GetEntity(i).f_Unlocked = true;
			}

			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				var entity = E_LevelGameplayData.GetEntity(i);
				entity.f_DebugComplete = true;

				if (E_LevelData.GetEntity(i).f_SegmentPresent && !entity.f_Completed)
				{
					persRef.serpProg.AddSegmentToDatabase();
				}

				if (E_LevelData.GetEntity(i).f_ObjectPresent && !entity.f_Completed)
				{
					persRef.objProg.AddObjectToDatabase();
				}
			}
		}

		public void UnlockAll()
		{
			for (int i = 0; i < E_BiomeGameplayData.CountEntities; i++)
			{
				E_BiomeGameplayData.GetEntity(i).f_Unlocked = true;	
			}

			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				var entity = E_LevelGameplayData.GetEntity(i);
				entity.f_LocksLeft = 0;
				entity.f_Unlocked = true;
				entity.f_LockIconDisabled = true;
				entity.f_UnlockAnimPlayed = true;

				for (int j = 0; j < E_LevelData.CountEntities; j++)
				{
					var levelEntity = E_LevelData.GetEntity(j);
					var unlockPins = levelEntity.f_UnlocksPins;

					if (unlockPins == null) continue;

					for (int k = 0; k < unlockPins.Count; k++)
					{
						var uPin = unlockPins[k];
						var uLevelEntity = E_LevelData.FindEntity(entity =>
							entity.f_Pin == uPin);
						var uGameplayEntity = E_LevelGameplayData.FindEntity(entity =>
							entity.f_Pin == uPin);

						if (uPin == entity.f_Pin)
						{
							var originEntity = E_LevelGameplayData.FindEntity(entity =>
								entity.f_Pin == levelEntity.f_Pin);
							
							originEntity.f_PathDrawn = true;
							originEntity.f_DottedAnimPlayed = true;
							originEntity.f_WallDown = true;
						}

						if (uGameplayEntity == null || uLevelEntity == null) continue;

						if (uLevelEntity.f_LocksAmount > 0 && 
							uGameplayEntity.f_LocksLeft != 0) uGameplayEntity.f_LocksLeft--;
					}
				}				
			}
		}

		public void CheckDebugStatuses()
		{
			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				var gameplayEntity = E_LevelGameplayData.GetEntity(i);

				DebugCompleteCheck(gameplayEntity);
				DebugUncompleteCheck(gameplayEntity);
			}
		}

		private static void DebugUncompleteCheck(E_LevelGameplayData gameplayEntity)
		{
			if (gameplayEntity.f_DebugUncomplete)
			{
				var levelEntity = E_LevelData.FindEntity(entity =>
					entity.f_Pin == gameplayEntity.f_Pin);

				gameplayEntity.f_Completed = false;
				gameplayEntity.f_WallDown = false;
				gameplayEntity.f_PathDrawn = false;
				gameplayEntity.f_DottedAnimPlayed = false;

				for (int j = 0; j < levelEntity.f_UnlocksPins.Count; j++)
				{
					var uPin = levelEntity.f_UnlocksPins[j];

					var uGameplayEntity = E_LevelGameplayData.FindEntity(entity =>
						entity.f_Pin == uPin);
					var uLevelEntity = E_LevelData.FindEntity(entity =>
						entity.f_Pin == uPin);

					if (uGameplayEntity == null || uLevelEntity == null) continue;

					uGameplayEntity.f_UnlockAnimPlayed = false;
					uGameplayEntity.f_Unlocked = false;
					uGameplayEntity.f_LockIconDisabled = false;

					if (uLevelEntity.f_LocksAmount > 0 && uLevelEntity.f_LocksAmount !=
						uGameplayEntity.f_LocksLeft) uGameplayEntity.f_LocksLeft++;
				}

				gameplayEntity.f_DebugUncomplete = false;
			}
		}

		private void DebugCompleteCheck(E_LevelGameplayData gameplayEntity)
		{
			if (gameplayEntity.f_DebugComplete)
			{
				if (!gameplayEntity.f_Unlocked) gameplayEntity.f_Unlocked = true;
				persRef.progHandler.SetLevelToComplete(gameplayEntity.f_Pin);
				gameplayEntity.f_DebugComplete = false;
			}
		}
	}
}
