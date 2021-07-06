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
		const string TO_ANGRY = "ToAngry";

		List<string> animStringList = new List<string>();

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			animStringList.Add(TO_LOW);
			animStringList.Add(TO_HIGH);
			animStringList.Add(TO_ANGRY);
		}

		public void SetBrows(BrowStates state)
		{
			SetCurrentBrow();

			foreach (var anim in animStringList)
			{
				animator.ResetTrigger(anim);
			}

			if (state == BrowStates.low) ToBaseAnim();

			if (state == BrowStates.high) ToFirstTierAnim(BrowStates.high, TO_HIGH);

			if (state == BrowStates.angry) ToFirstTierAnim(BrowStates.angry, TO_ANGRY);
		}

		private void SetCurrentBrow()
		{
			var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
			var currentClipName = currentClipInfo[0].clip.name;

			if (currentClipName == "Brow_Low" || currentClipName == "Brow_HighToLow" ||
				currentClipName == "Brow_AngryToLow") currentBrow = BrowStates.low;
			
			if (currentClipName == "Brow_LowToHigh") currentBrow = BrowStates.high;
			if (currentClipName == "Brow_LowToAngry") currentBrow = BrowStates.angry;
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
