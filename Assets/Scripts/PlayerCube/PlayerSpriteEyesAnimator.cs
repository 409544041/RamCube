using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerSpriteEyesAnimator : MonoBehaviour
	{
		//Cache
		Animator animator;

		//States
		EyesStates currentEyes = EyesStates.normal;
		const string TO_NORMAL = "ToNormal";
		const string TO_WINK = "ToWink";
		const string TO_SHUT = "ToShut";
		const string TO_LAUGH_SHUT = "ToLaughingShut";
		const string TO_ARCH = "ToArched";
		const string TO_LAUGH_ARCH = "ToLaughingArched";
		const string TO_TWITCH = "ToTwitch";
		const string TO_SHOCKED = "ToShocked";
		const string TO_CROSS = "ToCross";
		const string TO_SPARKLE = "ToSparkle";
		const string TO_SQUINT = "ToSquint";

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		//state1 is always the state you're going to.
		public void SetEyes(EyesStates state)
		{
			if (state == EyesStates.normal) ToBaseAnim();

			if (state == EyesStates.wink) 
				ToFirstTierAnim(EyesStates.wink, EyesStates.nullz, TO_WINK);

			if (state == EyesStates.shut) 
				ToFirstTierAnim(EyesStates.shut, EyesStates.laughShut, TO_SHUT);

			if (state == EyesStates.arched) 
				ToFirstTierAnim(EyesStates.arched, EyesStates.laughArched, TO_ARCH);

			if (state == EyesStates.twitch) 
				ToFirstTierAnim(EyesStates.twitch, EyesStates.nullz, TO_TWITCH);

			if (state == EyesStates.shock) 
				ToFirstTierAnim(EyesStates.shock, EyesStates.nullz, TO_SHOCKED);

			if (state == EyesStates.cross) 
				ToFirstTierAnim(EyesStates.cross, EyesStates.nullz, TO_CROSS);

			if (state == EyesStates.sparkle) 
				ToFirstTierAnim(EyesStates.sparkle, EyesStates.nullz, TO_SPARKLE);

			if (state == EyesStates.squint) 
				ToFirstTierAnim(EyesStates.squint, EyesStates.nullz, TO_SQUINT);
		}

		private void ToBaseAnim()
		{
			if (currentEyes == EyesStates.normal) return;

			if (currentEyes == EyesStates.laughArched) ToFirstTierAnim(EyesStates.arched, EyesStates.laughArched, TO_ARCH);
			if (currentEyes == EyesStates.laughShut) ToFirstTierAnim(EyesStates.shut, EyesStates.laughShut, TO_SHUT);

			animator.SetTrigger(TO_NORMAL);
			currentEyes = EyesStates.normal;
		}

		private void ToFirstTierAnim(EyesStates state1, EyesStates state2, string trigger)
		{
			if (currentEyes == state1) return;

			if (state2 == EyesStates.nullz) 
			{
				if (currentEyes != EyesStates.normal) ToBaseAnim();
			}
			else
			{
				if (currentEyes != EyesStates.normal && currentEyes != state2) ToBaseAnim();
			}

			animator.SetTrigger(trigger);
			currentEyes = state1;
		}

		private void ToSecondTierAnim(EyesStates state1, EyesStates state2, string trigger)
		{
			if (currentEyes == state1) return;

			if (currentEyes != EyesStates.normal && currentEyes != state2)
			{
				ToBaseAnim();

				if (state1 == EyesStates.laughShut)
					ToFirstTierAnim(EyesStates.shut, EyesStates.laughShut, TO_SHUT);

				if (state1 == EyesStates.laughArched)
					ToFirstTierAnim(EyesStates.arched, EyesStates.laughArched, TO_ARCH);
			}

			animator.SetTrigger(trigger);
			currentEyes = state1;
		}
	}
}
