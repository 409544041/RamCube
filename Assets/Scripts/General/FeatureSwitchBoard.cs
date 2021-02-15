using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.General
{
	public class FeatureSwitchBoard : MonoBehaviour
	{
		//Config parameters
		public bool serpentConnected;
		public bool worldMapConnected;

		//Cache
		FinishCube finish;

		private void Awake() 
		{
			finish = FindObjectOfType<FinishCube>();
		}

		private void OnEnable() 
		{
			if(finish != null)
			{
				finish.onSerpentCheck += FetchSerpentConnect;
				finish.onMapCheck += FetchMapConnect;
			} 
		}

		public bool FetchSerpentConnect()
		{
			return serpentConnected;
		}

		public bool FetchMapConnect()
		{
			return worldMapConnected;
		}

		private void OnDisable()
		{
			if (finish != null)
			{
				finish.onSerpentCheck -= FetchSerpentConnect;
				finish.onMapCheck -= FetchMapConnect;
			}
		}
	}
}
