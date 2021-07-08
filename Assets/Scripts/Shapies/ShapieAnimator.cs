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
		[SerializeField] Vector2 minMaxGetUpDelay, minMaxCelebrateDelay, minMaxPush;

		//Cache
		Animator animator;
		PlayerAnimator playerAnim;
		MMFeedbackPosition pushMMPos;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
			playerAnim = FindObjectOfType<PlayerAnimator>();
			pushMMPos = pushBackJuice.GetComponent<MMFeedbackPosition>();
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
			//Leaving 3rd push animation out on purpose bc it breaks end seq
			//bc it doesn't get to 'get up' which triggers player falling
			//TO DO: Fix this
			int randomPush = Random.Range(0, 2);
			animator.SetInteger("PushInt", randomPush);

			float randomPushDis = Random.Range(minMaxPush.x, minMaxPush.y);
			pushBackTarget.localPosition = new Vector3(0, 0, randomPushDis);

			//if using the 'walking back' animation, make push back slower
			if (randomPush == 2) pushMMPos.AnimatePositionDuration = 1.5f;
			else pushMMPos.AnimatePositionDuration = .7f;

			pushBackJuice.Initialization();
			pushBackJuice.PlayFeedbacks();
		}

		private IEnumerator TriggerGettingUp() //Called from animation event
		{
			var delay = Random.Range(minMaxGetUpDelay.x, minMaxGetUpDelay.y);
			yield return new WaitForSeconds(delay);
			animator.SetTrigger("GetUp");
		}

		private void TriggerShock() 
		{
			animator.SetTrigger("Shock");
		}

		private IEnumerator TriggerCelebration() //Called from animation event
		{
			var delay = Random.Range(minMaxCelebrateDelay.x, minMaxCelebrateDelay.y);
			yield return new WaitForSeconds(delay);
			animator.SetTrigger("Celebrate");
		}

		private IEnumerator TriggerPlayerFalling(float delay) //Called from animation event
		{
			yield return new WaitForSeconds(delay);
			playerAnim.TriggerFalling(0);
		}

		private void OnDisable()
		{
			if (playerAnim != null) playerAnim.onTriggerLandingReaction -= TriggerShock;
		}
	}
}
