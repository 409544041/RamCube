using Qbism.Dialogue;
using Qbism.General;
using Qbism.SpriteAnimations;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentRefHolder : MonoBehaviour
	{
		//Config parameters
		public M_Segments mSegments;
		public GameObject meshParent;
		public Renderer[] meshes;
		public SerpentSegmentHandler segHandler;
		[Header("Animation")]
		public Animator bodyAnimator;
		public ExpressionHandler exprHandler;
		public SegmentAnimator segAnim;
		public MotherDragonAnimator dragonAnim;
		[Header("Serpent Screen")]
		public SegmentScroll segScroll;
		[Header("Dialogue")]
		public DialogueStarter dialogueStarter;
		[Header("Map")]
		public ScreenDistanceShrinker distShrinker;
		public SerpentMapHandler serpMapHandler;
		[Header("UI")]
		public SegmentUIHandler uiHandler;
		public GameObject uiObject;
		public Canvas canvas;
		public CanvasGroup canvasGroup;
		public Transform markerTrans;
		public RectTransform notificationMarker;
		public RectTransform notificationArrow;

		//Cache
		public SerpCoreRefHolder scRef { get; set; }
		public GameplayCoreRefHolder gcRef { get; set; }
		public MapCoreRefHolder mcRef { get; set; }

		//States
		public Camera cam { get; set; }
	}
}
