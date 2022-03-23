using Qbism.Control;
using Qbism.Objects;
using Qbism.SceneTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SerpLogicRefHolder : MonoBehaviour
	{
		//Config parameters
		public SerpCoreRefHolder scRef;
		public SerpentScreenInputDetector input;
		public SerpentScreenScroller scroller;
		[Header("Level Loading")]
		public WorldMapLoading mapLoader;
		[Header("Object Handling")]
		public ObjectStatusChecker objStatusChecker;
		public ObjectUIHandler objUIHandler;
		public ObjectRenderSelector[] objRenderSelectors;

	}
}
