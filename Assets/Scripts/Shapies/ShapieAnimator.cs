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
		ShapieSoundHandler soundHandler;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
			playerAnim = FindObjectOfType<PlayerAnimator>();
			pushMMPos = pushBackJuice.GetComponent<MMFeedbackPosition>();
			soundHandler = GetComponentInParent<ShapieSoundHandler>();
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
			
			//Ensure default int in controller = -1 to avoid instant transitions
			int pushAnim = Random.Range(0, 2);
			animator.SetInteger("PushInt", pushAnim);

			float push = Random.Range(minMaxPush.x, minMaxPush.y);
			pushBackTarget.localPosition = new Vector3(0, 0, push);

			//if using the 'walking back' animation, make push back slower
			if (pushAnim == 2) pushMMPos.AnimatePositionDuration = 1.5f;
			else pushMMPos.AnimatePositionDuration = .7f;

			var soundMM = pushBackJuice.GetComponent<MMFeedbackSound>();
			soundMM.SetInitialDelay(Random.Range(0, .2f));

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
			//Ensure default int in controller = -1 to avoid instant transitions
			var delay = Random.Range(minMaxCelebrateDelay.x, minMaxCelebrateDelay.y);
			int celebAnim = Random.Range(0,2);
			yield return new WaitForSeconds(delay);
			animator.SetInteger("Celebrate", celebAnim);
		}

		private IEnumerator TriggerPlayerFalling(float delay) //Called from animation event
		{
			yield return new WaitForSeconds(delay);
			playerAnim.TriggerFall(false, "FallOnGround", Random.Range(-40, -50), false);
		}

		private void TriggerGibberishSlow()
		{
			soundHandler.currentIntervalMinMax = soundHandler.slowIntervalMinMax;
			soundHandler.PlayGibberish(true);
		}

		private void TriggerGibberishCelebration()
		{
			soundHandler.currentIntervalMinMax = soundHandler.fastIntervalMinMax;
			soundHandler.PlayGibberish(true);
		}

		private void OnDisable()
		{
			if (playerAnim != null) playerAnim.onTriggerLandingReaction -= TriggerShock;
		}
	}
}
