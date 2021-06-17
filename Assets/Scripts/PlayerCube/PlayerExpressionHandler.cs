using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerExpressionHandler : MonoBehaviour
	{
		//Config parameters
		public PlayerExpressionsScripOb expressionsSO;

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

		public void SetGameplayExpression(ExpressionsData[] expressions)
		{
			var expression = expressions[Random.Range(0, expressions.Length)];

			var mouth = expression.mouth;
			var eyes = expression.eyes;

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

		public void SetShockedExpression()
		{
			var mouth = expressionsSO.shocked.mouth;
			var eyes = expressionsSO.shocked.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetWailingPainExpression()
		{
			var mouth = expressionsSO.wailingPain.mouth;
			var eyes = expressionsSO.wailingPain.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetLovingItExpression()
		{
			var mouth = expressionsSO.lovingIt.mouth;
			var eyes = expressionsSO.lovingIt.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}

		public void SetAngryExpression()
		{
			var mouth = expressionsSO.angry.mouth;
			var eyes = expressionsSO.angry.eyes;

			mouthAnim.SetSprite(mouth, true);
			eyesAnim.SetSprite(eyes, true);
		}
	}
}
