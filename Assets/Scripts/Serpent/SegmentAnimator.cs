using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
using Qbism.PlayerCube;
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
		[SerializeField] float lookAroundAnimDelay = 0f, 
			lookUpAnimDelay = 0f, happyWiggleAnimDelay = 0f;

		//Cache
		SegmentExpressionHandler faceHandler = null;
		PlayerAnimator playerAnim = null;
		Animator playerAnimCont = null;

		//States
		bool justSpawned = false;

		private void Awake() 
		{
			faceHandler = GetComponent<SegmentExpressionHandler>();
			playerAnim = FindObjectOfType<PlayerAnimator>();
			playerAnimCont = playerAnim.GetComponent<Animator>();
		}

		private void OnEnable() 
		{
			if (playerAnim != null) playerAnim.onActivateSegmentSquish += TriggerSquish;	
		}

		public void Spawn()
		{
			justSpawned = true;
			spawnJuice.Initialization();
			spawnJuice.PlayFeedbacks();
			animator.SetTrigger("SpawnWiggle");
		}

		private IEnumerator TriggerLookAround() //Called from animation event
		{
			yield return new WaitForSeconds(lookAroundAnimDelay);
			animator.SetTrigger("LookAround");
		}

		private void TriggerSquish()
		{
			if (justSpawned) animator.SetTrigger("Squish");
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

		private IEnumerator TriggerPlayerLanding() //Called from animation event
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			playerAnimCont.SetTrigger("Fall");

			yield return null; //to ensure the player isn't on screen for 1 frame before starting anim

			player.transform.position = new Vector3(transform.position.x, 
				transform.position.y + 1, transform.position.z);
			
			player.transform.rotation = Quaternion.Euler(0f, -45f, 0f);
		}

		private void TriggerPlayerLookDown()
		{
			playerAnimCont.SetTrigger("LookDown");
		}

		private void TriggerPlayerWiggle() // Called from animation event
		{
			playerAnimCont.SetTrigger("Wiggle");
		}

		private void OnDisable()
		{
			if (playerAnim != null) playerAnim.onActivateSegmentSquish -= TriggerSquish;
		}
	}
}
