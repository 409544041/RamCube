using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Saving
{
	public class MapDelegateEnabler : MonoBehaviour
	{
		private void Awake() 
		{
			ProgressHandler progHandler = FindObjectOfType<ProgressHandler>();
			progHandler.FixMapDelegateLinks();
		}
	}
}
