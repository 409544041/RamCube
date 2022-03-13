﻿using Qbism.General;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class MapDelInitiator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MapCoreRefHolder mcRef;

		private void Awake() 
		{
			var persRef = mcRef.persRef;

			persRef.progHandler.FixMapUILinks();
			persRef.progHandler.FixMapPinLinks();
		}
	}
}
