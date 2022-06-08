using Qbism.Dialogue;
using Qbism.Objects;
using Qbism.SceneTransition;
using Qbism.ScreenStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SerpLogicRefHolder : MonoBehaviour
	{
		//Config parameters
		public SerpCoreRefHolder scRef;
		public ScreenStateManager screenStateMngr;
		public SerpentScreenScroller scroller;
		[Header("Level Loading")]
		public WorldMapLoading mapLoader;
		[Header("Object Handling")]
		public ObjectAndSegmentChecker objSegChecker;
		public SerpScreenUIHandler serpScreenUIHandler;
		public ObjectRenderSelector[] objRenderSelectors;
		[Header("Dialogue")]
		public DialogueManager dialogueManager;
		public DialogueFocuser dialogueFocuser;
	}
}
