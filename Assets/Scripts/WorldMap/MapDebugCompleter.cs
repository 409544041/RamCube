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
				var entity = E_LevelGameplayData.GetEntity(i);

				if (entity.f_DebugComplete)
				{
					if (!entity.f_Unlocked) entity.f_Unlocked = true;
					progHandler.SetLevelToComplete(entity.f_Pin);
					entity.f_DebugComplete = false;
				}
			}
		}
	}
}
