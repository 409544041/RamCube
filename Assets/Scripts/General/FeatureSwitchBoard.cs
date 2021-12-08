using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.General
{
	public class FeatureSwitchBoard : MonoBehaviour
	{
		//Config parameters
		[Header("Gameplay")]
		public bool showEndLevelSeq;
		public bool allowDebugFinish;
		public bool serpentConnected;
		public bool allowDebugLevelNav;
		[Header("World Map")]
		public bool worldMapConnected;
		public bool allowMapReload;
		public bool allowDebugCompleteAll;
		[Header("Demo")]
		public bool demoSplashConnected;
		[Header("Serpent Screen")]
		public bool serpentScreenConnected;

	}
}
