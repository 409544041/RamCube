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
			if (progHandler != null)
			{
				progHandler.FixMapUILinks();
				progHandler.FixMapPinLinks();
			}
			else Debug.Log("Progress Handler not found.");
		}
	}
}
