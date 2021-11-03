using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class ProgHandInitiator : MonoBehaviour
	{
		//Cache
		ProgressHandler progHandler;
		private void Awake() 
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			progHandler.FixMapUILinks();
			progHandler.BuildLevelPinList();
			progHandler.FixMapPinLinks();
		}

		private void Start() 
		{
			progHandler.HandleLevelPins();
		}
	}
}
