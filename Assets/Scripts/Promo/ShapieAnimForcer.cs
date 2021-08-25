using System.Collections;
using System.Collections.Generic;
using Qbism.Shapies;
using UnityEngine;

namespace Qbism.Promo
{
	public class ShapieAnimForcer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Vector2 minMaxLookDelay, minMaxCelebrateDelay;
		[SerializeField] bool forceCeleb01, forceCeleb02, forceLook;		

		//Cache
		Animator animator;
		ShapieSoundHandler soundHandler;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			soundHandler = GetComponentInParent<ShapieSoundHandler>();
		}

		public void ForceLookingAround()
		{
			if (forceLook) StartCoroutine(DebugLook());
		}

		private IEnumerator DebugLook()
		{
			var delay = Random.Range(minMaxLookDelay.x, minMaxLookDelay.y);
			yield return new WaitForSeconds(delay);
			animator.SetTrigger("Look");
		}

		public void ForceCelebrate()
		{
			if (forceCeleb01 || forceCeleb02) StartCoroutine(DebugCelebrate());
		}

		private IEnumerator DebugCelebrate()
		{
			var delay = Random.Range(minMaxCelebrateDelay.x, minMaxCelebrateDelay.y);
			yield return new WaitForSeconds(delay);
			if (forceCeleb01) animator.SetTrigger("Celebrate01");
			else if (forceCeleb02) animator.SetTrigger("Celebrate02");
		}

		private void TriggerGibberishCelebration()
		{
			soundHandler.currentIntervalMinMax = soundHandler.fastIntervalMinMax;
			soundHandler.PlayGibberish(true);
		}
	}
}
