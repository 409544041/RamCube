using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class MapDelInitiator : MonoBehaviour
	{

		//Cache
		ProgressHandler progHandler;
		SerpentProgress serpProg;
		
		private void Awake() 
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			serpProg = FindObjectOfType<SerpentProgress>();

			if (progHandler != null)
			{
				progHandler.FixMapUILinks();
				progHandler.FixMapPinLinks();
			}
			else Debug.Log("Progress Handler not found.");

			if (serpProg != null) serpProg.FixMapDelegateLinks();
			else Debug.Log("Serpent Progress not found.");
		}
	}
}
