using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Promo
{
	public class ShapieAnimForcer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Vector2 minMaxLookDelay, minMaxCelebrateDelay;
		[SerializeField] bool forceCelebration, forceLook;
		[Range(0,1)]
		[SerializeField] int celebAnim;
		

		//Cache
		Animator animator;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		public void ForceLookingAround()
		{
			if (forceLook) StartCoroutine(DebugLook());
		}

		private IEnumerator DebugLook()
		{
			var delay = Random.Range(minMaxLookDelay.x, minMaxLookDelay.y);
			yield return new WaitForSeconds(delay);
			animator.SetTrigger("LookAround");
		}

		public void ForceCelebrate()
		{
			if (forceCelebration) StartCoroutine(DebugCelebrate());
		}

		private IEnumerator DebugCelebrate()
		{
			var delay = Random.Range(minMaxCelebrateDelay.x, minMaxCelebrateDelay.y);
			yield return new WaitForSeconds(delay);
			animator.SetInteger("Celebrate", celebAnim);
		}
	}
}
