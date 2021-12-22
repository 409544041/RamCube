using System.Collections;
using System.Collections.Generic;
using Qbism.Environment;
using UnityEngine;

namespace Qbism.SpriteAnimations
{
	public class SpriteMouthAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool hasCircle, hasTeeth, hasWail, hasCry, hasBoop, hasYawn, hasLaugh;

		//Cache
		Animator animator;

		//States
		MouthStates currentMouth = MouthStates.normal;
		const string TO_NORMAL = "ToNormal";
		const string TO_CIRCLE = "ToCircle";
		const string TO_TEETH = "ToTeeth";
		const string TO_SAD = "ToSad";
		const string TO_WAIL = "ToWail";
		const string TO_SMILE = "ToSmile";
		const string TO_HAPPY = "ToHappyOpen";
		const string TO_CRY = "ToCry";
		const string TO_BOOP = "ToIdleBooping";
		const string TO_YAWN = "ToIdleYawn";
		const string TO_LAUGH = "ToLaughing";

		List<string> animStringList = new List<string>();

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		private void Start() 
		{
			animStringList.Add(TO_NORMAL);
			animStringList.Add(TO_CIRCLE);
			animStringList.Add(TO_TEETH);
			animStringList.Add(TO_SAD);
			animStringList.Add(TO_WAIL);
			animStringList.Add(TO_SMILE);
			animStringList.Add(TO_HAPPY);
			animStringList.Add(TO_CRY);
			animStringList.Add(TO_BOOP);
			animStringList.Add(TO_YAWN);
			animStringList.Add(TO_LAUGH); 
		}

		//state1 is always the state you're going to.
		//Order of state2 and 3 are from center of animator outwards 

		public void SetMouth(MouthStates state)
		{
			foreach (var anim in animStringList)
			{
				animator.ResetTrigger(anim);
			}

			SetCurrentMouth();

			if (state == MouthStates.normal) ToBaseAnim();

			if (state == MouthStates.circle && hasCircle) ToFirstTierAnim(MouthStates.circle,
				MouthStates.nullz, MouthStates.nullz, TO_CIRCLE);

			else if (state == MouthStates.circle && !hasCircle && hasWail)
				ToSecondTierAnim(MouthStates.wail, MouthStates.sad, MouthStates.cry, TO_WAIL);

			else if (state == MouthStates.circle && !hasCircle && !hasWail)
				ToFirstTierAnim(MouthStates.sad, MouthStates.wail, MouthStates.cry, TO_SAD);

			if (state == MouthStates.teeth && hasTeeth) ToFirstTierAnim(MouthStates.teeth,
				MouthStates.nullz, MouthStates.nullz, TO_TEETH);

			else if (state == MouthStates.teeth && !hasTeeth) 
				Debug.LogError("Character does not have teeth mouth animation.");

			if (state == MouthStates.sad) ToFirstTierAnim(MouthStates.sad,
				MouthStates.wail, MouthStates.cry, TO_SAD);

			if (state == MouthStates.wail && hasWail) ToSecondTierAnim(MouthStates.wail,
				MouthStates.sad, MouthStates.cry, TO_WAIL);

			else if (state == MouthStates.wail && !hasWail) ToFirstTierAnim(MouthStates.sad,
				MouthStates.wail, MouthStates.cry, TO_SAD);

			if (state == MouthStates.smile) ToFirstTierAnim(MouthStates.smile,
				MouthStates.happyOpen, MouthStates.laugh, TO_SMILE);

			if (state == MouthStates.happyOpen) ToSecondTierAnim(MouthStates.happyOpen,
				MouthStates.smile, MouthStates.laugh, TO_HAPPY);

			if (state == MouthStates.laugh && hasLaugh) ToThirdTierAnim(MouthStates.laugh,
				MouthStates.smile, MouthStates.happyOpen, TO_LAUGH);

			else if (state == MouthStates.laugh && !hasLaugh)
				Debug.LogError("Character does not have laughing mouth animation.");

			if (state == MouthStates.cry && hasCry) ToThirdTierAnim(MouthStates.cry,
				MouthStates.sad, MouthStates.wail, TO_CRY);

			if (state == MouthStates.cry && !hasCry)
				Debug.LogError("Character does not have cry mouth animation");

			if (state == MouthStates.boop && hasBoop) ToFirstTierAnim(MouthStates.boop, 
					MouthStates.nullz, MouthStates.nullz, TO_BOOP);

			else if (state == MouthStates.boop && !hasBoop)
				Debug.LogError("Character does not have booping mouth animation.");

			if (state == MouthStates.yawn && hasYawn) ToFirstTierAnim(MouthStates.yawn, 
				MouthStates.nullz, MouthStates.nullz, TO_YAWN);

			else if (state == MouthStates.yawn && !hasYawn)
				Debug.LogError("Character does not have yawning mouth animation.");
		}

		private void SetCurrentMouth()
		{
			var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
			var currentClipName = currentClipInfo[0].clip.name;

			if (currentClipName == "Mouth_Normal" || currentClipName == "Mouth_CircleToNormal" ||
				currentClipName == "Mouth_TeethToNormal" || currentClipName == "Mouth_SadToNormal" ||
				currentClipName == "Mouth_SmileToNormal") currentMouth = MouthStates.normal;
			
			if (currentClipName == "Mouth_NormalToCircle") currentMouth = MouthStates.circle;
			if (currentClipName == "Mouth_NormalToTeeth") currentMouth = MouthStates.teeth;
			if (currentClipName == "Mouth_NormalToSad" || currentClipName == "Mouth_WailToSad") currentMouth = MouthStates.sad;
			if (currentClipName == "Mouth_NormalToSmile" || currentClipName == "Mouth_HappyOpenToSmile") currentMouth = MouthStates.smile;

			if (currentClipName == "Mouth_SadToWail" || currentClipName == "Mouth_Wail") currentMouth = MouthStates.wail;
			if (currentClipName == "Mouth_SmileToHappyOpen") currentMouth = MouthStates.happyOpen;
			if (currentClipName == "Mouth_Laughing") currentMouth = MouthStates.laugh;

			if (currentClipName == "Mouth_Crying") currentMouth = MouthStates.cry;
			if (currentClipName == "Mouth_IdleBooping") currentMouth = MouthStates.boop;
			if (currentClipName == "Mouth_IdleYawning")	currentMouth = MouthStates.yawn;
		}

		private void ToBaseAnim()
		{
			if (currentMouth == MouthStates.normal) return;

			if (currentMouth == MouthStates.cry)
				ToSecondTierAnim(MouthStates.wail, MouthStates.sad, 
				MouthStates.cry, TO_WAIL);

			if (currentMouth == MouthStates.laugh)
				ToSecondTierAnim(MouthStates.happyOpen, MouthStates.smile, 
				MouthStates.laugh, TO_HAPPY);

			if (currentMouth == MouthStates.wail)
				ToFirstTierAnim(MouthStates.sad, MouthStates.wail,
				MouthStates.cry, TO_SAD);

			if (currentMouth == MouthStates.happyOpen) 
				ToFirstTierAnim(MouthStates.smile, MouthStates.happyOpen,
				MouthStates.laugh, TO_SMILE);

			animator.SetTrigger(TO_NORMAL);
			currentMouth = MouthStates.normal;
		}

		private void ToFirstTierAnim(MouthStates state1, MouthStates state2, 
			MouthStates state3, string trigger1)
		{
			if (currentMouth == state1) return;

			if (state2 == MouthStates.nullz && state3 == MouthStates.nullz)
			{
				if (currentMouth != MouthStates.normal) ToBaseAnim();

				animator.SetTrigger(trigger1);
				currentMouth = state1;
			}
			else
			{
				if (currentMouth != MouthStates.normal && currentMouth != state2 &&
				currentMouth != state3) ToBaseAnim();

				if (currentMouth == state3)
				{
					string newTrigger = null;
					if (state1 == MouthStates.sad) newTrigger = TO_WAIL;
					if (state1 == MouthStates.smile) newTrigger = TO_HAPPY;

					ToSecondTierAnim(state2, state1, state3, newTrigger);
				}

				animator.SetTrigger(trigger1);
				currentMouth = state1;
			}
		}

		private void ToSecondTierAnim(MouthStates state1, MouthStates state2, 
			MouthStates state3, string trigger)
		{
			if (currentMouth == state1) return;

			if (currentMouth != MouthStates.normal && currentMouth != state2 &&
				currentMouth != state3)
				ToBaseAnim();

			string newTrigger = null;
			if (state1 == MouthStates.wail) newTrigger = TO_SAD;
			if (state1 == MouthStates.happyOpen) newTrigger = TO_SMILE;

			if (currentMouth == MouthStates.normal) 
				ToFirstTierAnim(state2, state1, state3, newTrigger);

			animator.SetTrigger(trigger);
			currentMouth = state1;
		}

		private void ToThirdTierAnim(MouthStates state1, MouthStates state2,
			MouthStates state3, string trigger)
		{
			if (currentMouth == state1) return;

			if (currentMouth != MouthStates.normal && currentMouth != state2 &&
				currentMouth != state3)
				ToBaseAnim();

			string newTrigger = null;
			if (state1 == MouthStates.cry) newTrigger = TO_SAD;
			if (state1 == MouthStates.laugh) newTrigger = TO_SMILE;

			if (currentMouth == MouthStates.normal)
				ToFirstTierAnim(state2, state1, state3, newTrigger);

			if (state1 == MouthStates.cry) newTrigger = TO_WAIL;
			if (state1 == MouthStates.laugh) newTrigger = TO_HAPPY;

			if (currentMouth == state2)
				ToSecondTierAnim(state3, state2, state1, newTrigger);

			animator.SetTrigger(trigger);
			currentMouth = state1;
		}
	}
}
