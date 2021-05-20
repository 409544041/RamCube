using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Shapies
{
	public class ShapieAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Transform pushBackTarget;
		[SerializeField] MMFeedbacks pushBackJuice;
		[SerializeField] float minGetUpDelay, maxGetUpDelay, 
			minCelebrateDelay, maxCelebrateDelay;

		//Cache
		Animator animator;
		PlayerAnimator playerAnim;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
			playerAnim = FindObjectOfType<PlayerAnimator>();
		}

		private void OnEnable() 
		{
			if (playerAnim != null) playerAnim.onTriggerLandingReaction += TriggerShock;
		}

		private void Start() 
		{
			PushBack();
		}

		public void PushBack()
		{
			int randomPush = Random.Range(0, 2);
			animator.SetInteger("PushInt", randomPush);

			pushBackJuice.Initialization();
			pushBackJuice.PlayFeedbacks();
		}

		private IEnumerator TriggerGettingUp() //Called from animation event
		{
			var delay = Random.Range(minGetUpDelay, maxGetUpDelay);
			yield return new WaitForSeconds(delay);
			animator.SetTrigger("GetUp");
		}

		private void TriggerShock() 
		{
			animator.SetTrigger("Shock");
		}

		private IEnumerator TriggerCelebration() //Called from animation event
		{
			var delay = Random.Range(minCelebrateDelay, maxCelebrateDelay);
			yield return new WaitForSeconds(delay);
			animator.SetTrigger("Celebrate");
		}

		private IEnumerator TriggerPlayerFalling(float delay) //Called from animation event
		{
			yield return new WaitForSeconds(delay);
			playerAnim.TriggerFalling(0f);
		}

		private void OnDisable()
		{
			if (playerAnim != null) playerAnim.onTriggerLandingReaction -= TriggerShock;
		}
	}
}
