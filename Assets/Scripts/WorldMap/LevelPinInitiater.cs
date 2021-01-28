using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinInitiater : MonoBehaviour
	{
		void Start()
		{
			ProgressHandler progHandler = FindObjectOfType<ProgressHandler>();
			progHandler.levelPinList.Clear();
			progHandler.BuildLevelPinList();
			progHandler.FixDelegateLink();
			progHandler.HandleLevelPins();
		}
	}
}
