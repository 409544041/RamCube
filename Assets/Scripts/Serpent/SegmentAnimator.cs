using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
using Qbism.Dialogue;
using Qbism.PlayerCube;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentAnimator : MonoBehaviour
	{
		//Config paramters
		[Header("Juice")]
		[SerializeField] MMFeedbacks spawnJuice;
		[SerializeField] MMFeedbacks flybyJuice;
		[Header("Animation")]
		[SerializeField] float lookUpAnimDelay = 0f, happyWiggleAnimDelay = 0f;
		[SerializeField] SegmentRefHolder refs;

		//Cache
		PlayerAnimator playerAnim;

		//States
		public bool justSpawned { get; private set; } = false;

		public void Spawn()
		{
			playerAnim = refs.gcRef.pRef.playerAnim;

			justSpawned = true;
			spawnJuice.Initialization();
			spawnJuice.PlayFeedbacks();
			refs.bodyAnimator.SetTrigger("SpawnWiggle");
		}

		public void ChildToPlayer()
		{
			if (justSpawned)
			{
				var player = refs.gcRef.pRef.playerMover.transform;
				var myParent = transform.parent;
				myParent.transform.parent = player;
			}
		}

		public void ActivateFlyByJuice()
		{
			flybyJuice.Initialization();
			flybyJuice.PlayFeedbacks();
		}

		public void TriggerSquish()
		{
			if (justSpawned) refs.bodyAnimator.SetTrigger("Squish");
		}

		private IEnumerator TriggerLookUp() //Called from animation event
		{
			//Take the player landing + squish reaction duration into account for look up delay
			yield return new WaitForSeconds(lookUpAnimDelay);
			refs.bodyAnimator.SetTrigger("LookUp");
		}

		public void InitiateHappyWiggle()
		{
			StartCoroutine(TriggerHappyWiggle());
		}

		private IEnumerator TriggerHappyWiggle() 
		{
			yield return new WaitForSeconds(happyWiggleAnimDelay);
			refs.bodyAnimator.SetTrigger("HappyWiggle");
		}

		private void TriggerPlayerLanding() //Called from animation event
		{
			playerAnim.TriggerFall(true, "FallOnSegment", -45, false);
		}

		private void TriggerPlayerLookDown() //Called from animation event
		{
			playerAnim.TriggerLookDown();
		}

		private void TriggerPlayerWiggle() // Called from animation event
		{
			playerAnim.TriggerWiggle();
		}

		private void StartRescueDialogue() // Called from animation event
		{
			refs.dialogueStarter.StartRescueDialogue(this);
		}

		private void TriggerAfterDialogueCam() // Called from animation event
		{
			refs.gcRef.finishRef.endSeq.PanAndZoomCamAfterDialogue();
		}
	}
}
