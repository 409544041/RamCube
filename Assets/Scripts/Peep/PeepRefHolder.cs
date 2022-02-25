using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepRefHolder : MonoBehaviour
	{
		public PeepStateManager stateMngr;
		public PeepIdleState idleState;
		public PeepWalkState walkState;
		public PeepRunState runState;
		public PeepHideState hideState;
		public PeepInvestigateState investigateState;
		public NavMeshAgent agent;
		public Animator animator;
		public PeepAnimator peepAnim;
		public PeepExpressionHandler expressionHandler;
		public PeepMover peepMover;
		public MMFeedbacks hideJuice, shockUIJuice, questionUIjuice;
		public CanvasGroup[] expressionSignals;
		public Renderer[] meshes;
	}
}
