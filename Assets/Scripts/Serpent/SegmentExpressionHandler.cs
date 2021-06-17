using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentExpressionHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SegmentExpressionsScripOb expressionsSO;

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
			var mouth = expressionsSO.neutral.mouth;
			var eyes = expressionsSO.neutral.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetContentExpression()
		{
			var mouth = expressionsSO.content.mouth;
			var eyes = expressionsSO.content.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetSadExpression()
		{
			var mouth = expressionsSO.sad.mouth;
			var eyes = expressionsSO.sad.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetHappySmileExpression()
		{
			var mouth = expressionsSO.happySmile.mouth;
			var eyes = expressionsSO.happySmile.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetLaughingLoudExpression()
		{
			var mouth = expressionsSO.laughingAloud.mouth;
			var eyes = expressionsSO.laughingAloud.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetVeryHappyExpression()
		{
			var mouth = expressionsSO.veryHappy.mouth;
			var eyes = expressionsSO.veryHappy.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetSlightlyPainfulExpression()
		{
			var mouth = expressionsSO.slightlyPainful.mouth;
			var eyes = expressionsSO.slightlyPainful.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetVeryPainfulExpression()
		{
			var mouth = expressionsSO.veryPainful.mouth;
			var eyes = expressionsSO.veryPainful.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetAnnoyedExpression()
		{
			var mouth = expressionsSO.annoyed.mouth;
			var eyes = expressionsSO.annoyed.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetLookingUpSadExpression()
		{
			var mouth = expressionsSO.lookingUpSad.mouth;
			var eyes = expressionsSO.lookingUpSad.eyes;

			mouthAnim.SetSprite(mouth, false);
			eyesAnim.SetSprite(eyes, false);
		}

		public void SetLookingUpShockedExpression()
		{
			var mouth = expressionsSO.lookingUpShocked.mouth;
			var eyes = expressionsSO.lookingUpShocked.eyes;

			mouthAnim.SetSprite(mouth, false);
			eyesAnim.SetSprite(eyes, false);
		}

		public void SetLookConfusedLeftExpression()
		{
			var mouth = expressionsSO.confusedLeft.mouth;
			var eyes = expressionsSO.confusedLeft.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetLookConfusedRightExpression()
		{
			var mouth = expressionsSO.confusedRight.mouth;
			var eyes = expressionsSO.confusedRight.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}
	}
}
