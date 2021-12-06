using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class MapDebugCompleter : MonoBehaviour
	{
		//Cache
		ProgressHandler progHandler;

		private void Awake() 
		{
			progHandler = FindObjectOfType<ProgressHandler>();
		}

		public void CheckDebugCompletes()
		{
			for (int i = 0; i < E_LevelGameplayData.CountEntities; i++)
			{
				var gameplayEntity = E_LevelGameplayData.GetEntity(i);

				if (gameplayEntity.f_DebugComplete)
				{
					if (!gameplayEntity.f_Unlocked) gameplayEntity.f_Unlocked = true;
					progHandler.SetLevelToComplete(gameplayEntity.f_Pin);
					gameplayEntity.f_DebugComplete = false;
				}

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
												
						uGameplayEntity.f_UnlockAnimPlayed = false;
						uGameplayEntity.f_Unlocked = false;
						uGameplayEntity.f_LockIconDisabled = false;

						if (uLevelEntity.f_LocksAmount > 0 && uLevelEntity.f_LocksAmount !=
							uGameplayEntity.f_LocksLeft) uGameplayEntity.f_LocksLeft++;
					}

					gameplayEntity.f_DebugUncomplete = false;
				}
			}
		}
	}
}
