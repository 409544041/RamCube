using Qbism.Dialogue;
using Qbism.General;
using Qbism.SpriteAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentRefHolder : MonoBehaviour
	{
		//Config parameters
		public M_Segments mSegments;
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

		//Cache
		public SerpCoreRefHolder scRef { get; set; }
		public GameplayCoreRefHolder gcRef { get; set; }
	}
}
