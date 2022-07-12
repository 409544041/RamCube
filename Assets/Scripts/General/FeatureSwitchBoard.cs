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
		public bool showLevelName;
		public bool showFirstLevelIntroRoll;
		[Header("World Map")]
		public bool worldMapConnected;
		public bool allowDebugMapReload;
		public bool allowDebugCompleteAll;
		public bool allowDebugUnlockAll;
		public bool allowDebugDeleteProgress;
		[Header("Dialogue")]
		public bool triggerMapDialogue;
		[Header("Demo")]
		public bool demoSplashConnected;
		public bool showDebugTextInfo;
		public bool isPublicDemo;
		[Header("Serpent Screen")]
		public bool serpentScreenConnected;
		[Header("Object Collection")]
		public bool objectsConnected;
		[Header("For Trailer")]
		public bool allowHudToggle;
		public bool allowCubeUIToggle;
		[Header("Splash")]
		public bool showLogos;
		[Header("Build")]
		[Tooltip("in format major.minor.revision.buildnumber")]
		public string currentBuild;
		//major is usually many new features, changes in UI, release state
		//minor is perhaps some new features on a previous major release
		//revision is usually fixes on a previous minor release.
		//buildnumber is incremented for each latest build of a revision
	}
}
