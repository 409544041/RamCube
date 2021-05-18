using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentAnimator : MonoBehaviour
	{
		//Config paramters
		[Header ("Spawn Juice")]
		[SerializeField] MMFeedbacks spawnJuice = null;
		[Header ("Animation")]
		[SerializeField] Animator animator = null;
		[SerializeField] float spawnAnimDelay = .2f, lookAroundAnimDelay = 0f, 
			lookUpAnimDelay = 0f, happyWiggleAnimDelay = 0f;

		//Cache
		SegmentExpressionHandler faceHandler = null;

		private void Awake() 
		{
			faceHandler = GetComponent<SegmentExpressionHandler>();
		}

		public void Spawn()
		{
			StartCoroutine(TriggerSpawnSequence());
		}
		
		private IEnumerator TriggerSpawnSequence()
		{
			spawnJuice.Initialization();
			spawnJuice.PlayFeedbacks();
			yield return new WaitForSeconds(spawnAnimDelay);
			animator.SetTrigger("SpawnWiggle");
		}

		private IEnumerator TriggerLookAround() //Called from animation event
		{
			yield return new WaitForSeconds(lookAroundAnimDelay);
			animator.SetTrigger("LookAround");
		}

		private void TriggerSquish()
		{
			animator.SetTrigger("Squish");
		}

		private IEnumerator TriggerLookUp() //Called from animation event
		{
			//Take the player landing + squish reaction duration into account for look up delay
			yield return new WaitForSeconds(lookUpAnimDelay);
			animator.SetTrigger("LookUp");
		}

		private IEnumerator TriggerHappyWiggle() //Called from animation event
		{
			yield return new WaitForSeconds(happyWiggleAnimDelay);
			animator.SetTrigger("HappyWiggle");
		}

		private void ActivateSerpent()
		{
			var finish = FindObjectOfType<FinishCube>();
			finish.InitiateSerpentSequence();
		}

		// TO DO: CALL PLAYER LANDING ANIM
		// private void ActivatePlayerLanding() //Called from animation event
		// {
		// 	var finishJuicer = FindObjectOfType<FinishCubeJuicer>();
		// 	finishJuicer.ActivatePlayerLanding();
		// }
	}
}
