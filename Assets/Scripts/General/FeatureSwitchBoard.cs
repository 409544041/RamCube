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
		public bool serpentConnected;
		public bool allowDebugFinish;
		public bool allowDebugLevelNav;
		[Header("World Map")]
		public bool worldMapConnected;
		public bool allowDebugMapReload;
		public bool allowDebugCompleteAll;
		public bool allowDebugUnlockAll;
		public bool allowDebugDeleteProgress;
		[Header("Demo")]
		public bool demoSplashConnected;
		public bool isPublicDemo;
		[Header("Serpent Screen")]
		public bool serpentScreenConnected;
		[Header("Object Collection")]
		public bool objectsConnected;
		[Header("For Trailer")]
		public bool allowHudToggle;
	}
}
