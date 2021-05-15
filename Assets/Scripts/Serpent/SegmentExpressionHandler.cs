using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentExpressionHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SegmentExpressionsScripOb expressionsScripOb;

		//Cache
		SegmentSpriteAnimator mouthAnim = null;
		SegmentSpriteAnimator eyesAnim = null;

		private void Awake()
		{
			SegmentSpriteAnimator[] spriteAnims = GetComponentsInChildren<SegmentSpriteAnimator>();

			foreach (SegmentSpriteAnimator spriteAnim in spriteAnims)
			{
				if (spriteAnim.spriteID == SegmentSpriteIDs.mouth)
					mouthAnim = spriteAnim;

				else if (spriteAnim.spriteID == SegmentSpriteIDs.eyes)
					eyesAnim = spriteAnim;
			}
		}

		public void SetNeutralExpression()
		{
			var mouth = expressionsScripOb.neutral.mouth;
			var eyes = expressionsScripOb.neutral.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetContentExpression()
		{
			var mouth = expressionsScripOb.content.mouth;
			var eyes = expressionsScripOb.content.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetSadExpression()
		{
			var mouth = expressionsScripOb.sad.mouth;
			var eyes = expressionsScripOb.sad.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetHappySmileExpression()
		{
			var mouth = expressionsScripOb.happySmile.mouth;
			var eyes = expressionsScripOb.happySmile.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetLaughingLoudExpression()
		{
			var mouth = expressionsScripOb.laughingAloud.mouth;
			var eyes = expressionsScripOb.laughingAloud.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetVeryHappyExpression()
		{
			var mouth = expressionsScripOb.veryHappy.mouth;
			var eyes = expressionsScripOb.veryHappy.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetSlightlyPainfulExpression()
		{
			var mouth = expressionsScripOb.slightlyPainful.mouth;
			var eyes = expressionsScripOb.slightlyPainful.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetVeryPainfulExpression()
		{
			var mouth = expressionsScripOb.veryPainful.mouth;
			var eyes = expressionsScripOb.veryPainful.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetAnnoyedExpression()
		{
			var mouth = expressionsScripOb.annoyed.mouth;
			var eyes = expressionsScripOb.annoyed.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetLookingUpSadExpression()
		{
			var mouth = expressionsScripOb.lookingUpSad.mouth;
			var eyes = expressionsScripOb.lookingUpSad.eyes;

			mouthAnim.SetSprite(mouth, false);
			eyesAnim.SetSprite(eyes, false);
		}

		public void SetLookingUpShockedExpression()
		{
			var mouth = expressionsScripOb.lookingUpShocked.mouth;
			var eyes = expressionsScripOb.lookingUpShocked.eyes;

			mouthAnim.SetSprite(mouth, false);
			eyesAnim.SetSprite(eyes, false);
		}

		public void SetLookConfusedLeftExpression()
		{
			var mouth = expressionsScripOb.confusedLeft.mouth;
			var eyes = expressionsScripOb.confusedLeft.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetLookConfusedRightExpression()
		{
			var mouth = expressionsScripOb.confusedRight.mouth;
			var eyes = expressionsScripOb.confusedRight.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}
	}
}
