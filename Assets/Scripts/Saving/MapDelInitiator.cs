using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class MapDelInitiator : MonoBehaviour
	{
		private void Awake() 
		{
			var progHandler = FindObjectOfType<ProgressHandler>();
			var matHandler = progHandler.GetComponent<VarietyMaterialHandler>();

			progHandler.FixMapUILinks();
			progHandler.FixMapPinLinks();
			matHandler.FixLinks();
		}
	}
}
