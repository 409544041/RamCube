using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.SpriteAnimations
{
	public class SpriteBrowAnimator : MonoBehaviour
	{
		//Cache
		Animator animator;

		//States
		BrowStates currentBrow = BrowStates.low;
		const string TO_LOW = "ToLow";
		const string TO_HIGH = "ToHigh";
		const string TO_FROWN = "ToFrown";
		const string TO_ANGRY = "ToAngry";
		const string TO_HIGHLAUGH = "ToHighLaugh";

		List<string> animStringList = new List<string>();

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			animStringList.Add(TO_LOW);
			animStringList.Add(TO_HIGH);
			animStringList.Add(TO_FROWN);
			animStringList.Add(TO_ANGRY);
			animStringList.Add(TO_HIGHLAUGH);
		}

		//state1 is always the state you're going to.
		public void SetBrows(BrowStates state)
		{
			SetCurrentBrow();

			foreach (var anim in animStringList)
			{
				animator.ResetTrigger(anim);
			}

			if (state == BrowStates.low) ToBaseAnim();

			if (state == BrowStates.high) ToFirstTierAnim(BrowStates.high, BrowStates.nullz, TO_HIGH);

			if (state == BrowStates.frown) ToFirstTierAnim(BrowStates.frown, BrowStates.angry, TO_FROWN);

			if (state == BrowStates.angry) ToSecondTierAnim(BrowStates.angry, BrowStates.frown, TO_ANGRY);

			if (state == BrowStates.highLaugh) ToSecondTierAnim(BrowStates.highLaugh, BrowStates.high, TO_HIGHLAUGH);
		}

		private void SetCurrentBrow()
		{
			var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
			var currentClipName = currentClipInfo[0].clip.name;

			if (currentClipName == "Brow_Low" || currentClipName == "Brow_HighToLow") 
				currentBrow = BrowStates.low;

			if (currentClipName == "Brow_Frown") currentBrow = BrowStates.frown;
			if (currentClipName == "Brow_Angry") currentBrow = BrowStates.angry;			
			if (currentClipName == "Brow_LowToHigh" || currentClipName == "Brow_High") currentBrow = BrowStates.high;
			if (currentClipName == "Brow_HighLaugh") currentBrow = BrowStates.highLaugh;
			
		}

		private void ToBaseAnim()
		{
			if (currentBrow == BrowStates.low) return;

			if (currentBrow == BrowStates.angry) 
				ToFirstTierAnim(BrowStates.frown, BrowStates.angry, TO_FROWN);
			
			animator.SetTrigger(TO_LOW);
			currentBrow = BrowStates.low;
		}

		private void ToFirstTierAnim(BrowStates state1, BrowStates state2, string trigger)
		{
			if (currentBrow == state1) return;

			if (state2 == BrowStates.nullz)
			{
				if (currentBrow != BrowStates.low) ToBaseAnim();
			}
			else
			{
				if (currentBrow != BrowStates.low && currentBrow != state2) ToBaseAnim();
			}

			animator.SetTrigger(trigger);
			currentBrow = state1;
		}

		private void ToSecondTierAnim(BrowStates state1, BrowStates state2, string trigger)
		{
			if (currentBrow == state1) return;

			if (currentBrow != BrowStates.low && currentBrow != state2)
			{
				ToBaseAnim();
				ToFirstTierAnim(state1, state2, TO_ANGRY);
			}

			animator.SetTrigger(trigger);
			currentBrow = state1;
		}
	}
}
