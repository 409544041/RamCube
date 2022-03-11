using Qbism.General;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class MapDelInitiator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MapCoreRefHolder mapCoreRef;

		private void Awake() 
		{
			var persRef = mapCoreRef.persistantRef;

			persRef.progHandler.FixMapUILinks();
			persRef.progHandler.FixMapPinLinks();
			persRef.varMatHandler.FixLinks();
		}
	}
}
