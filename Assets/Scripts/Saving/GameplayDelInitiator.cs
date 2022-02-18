using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class GameplayDelInitiator : MonoBehaviour
	{
		private void Awake()
		{
			var matHandler = FindObjectOfType<VarietyMaterialHandler>();
			if (matHandler != null) matHandler.FixLinks();
		}
	}
}
