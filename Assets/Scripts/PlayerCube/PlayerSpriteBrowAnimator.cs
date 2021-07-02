using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerSpriteBrowAnimator : MonoBehaviour
	{
		//Cache
		Animator animator;

		//States
		BrowStates currentBrow = BrowStates.low;
		const string TO_LOW = "ToLow";
		const string TO_HIGH = "ToHigh";
		const string TO_ANGRY = "ToAngry"; 

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		public void SetBrows(BrowStates state)
		{
			if (state == BrowStates.low) ToBaseAnim();

			if (state == BrowStates.high) ToFirstTierAnim(BrowStates.high, TO_HIGH);

			if (state == BrowStates.angry) ToFirstTierAnim(BrowStates.angry, TO_ANGRY);
		}

		private void ToBaseAnim()
		{
			if (currentBrow == BrowStates.low) return;
			
			animator.SetTrigger(TO_LOW);
			currentBrow = BrowStates.low;
		}

		private void ToFirstTierAnim(BrowStates state, string trigger)
		{
			if (currentBrow == state) return;

			if (currentBrow != BrowStates.low) ToBaseAnim();

			animator.SetTrigger(trigger);
			currentBrow = state;
		}
	}
}
