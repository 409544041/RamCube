using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

namespace Qbism.Peep
{
	public class PeepRefHolder : MonoBehaviour
	{
		[Header("States")]
		public PeepStateManager stateMngr;
		public PeepIdleState idleState;
		public PeepWalkState walkState;
		public PeepRunState runState;
		public PeepHideState hideState;
		public PeepInvestigateState investigateState;
		public PeepCowerState cowerState;
		public PeepBalloonIdleState balloonIdleState;
		[Header("Pathfinding")]
		public RichAI aiRich;
		public Seeker pathSeeker;
		[Header("Animation")]
		public Animator animator;
		public PeepAnimator peepAnim;
		[Header("Expressions")]
		public PeepExpressionHandler expressionHandler;
		public CanvasGroup[] expressionSignals;
		[Header("Juice")]
		public MMFeedbacks hideJuice, shockUIJuice, questionUIjuice;
		[Header("Meshes")]
		public Renderer[] meshes;

		//Cache
		public Camera cam { get; set; }
	}
}
