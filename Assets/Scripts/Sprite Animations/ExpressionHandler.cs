using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.SpriteAnimations
{
	public class ExpressionHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] ExpressionsScripOb expressionsSO;
		[SerializeField] SituationsExprsScripOb situationsExprsSO;
		[SerializeField] bool isPlayer;
		[SerializeField] bool hasBrows;
		[SerializeField] Vector2 minMaxExpressionTime;

		//Cache
		SpriteBrowAnimator browAnim = null;
		SpriteEyesAnimator eyesAnim = null;
		SpriteMouthAnimator mouthAnim = null;

		//States
		float expressionTimer = 0f;
		float timeToExpress = 0f;
		public bool hasFinished { get; set; } = false;

		private void Awake()
		{
			browAnim = GetComponentInChildren<SpriteBrowAnimator>();
			eyesAnim = GetComponentInChildren<SpriteEyesAnimator>();
			mouthAnim = GetComponentInChildren<SpriteMouthAnimator>();
		}

		private void Update()
		{
			if (isPlayer && !hasFinished) HandleExpressionTimer();
		}

		public void SetSituationFace(ExpressionSituations incSituation, float incTime)
		{
			if (isPlayer)
			{
				foreach (var situationExpr in situationsExprsSO.situationExpressions)
				{
					if (situationExpr.situation != incSituation) continue;

					int index = UnityEngine.Random.Range(0, situationExpr.expressions.Length);
					var expressionToSet = situationExpr.expressions[index];

					SetFace(expressionToSet, incTime);
				}
			}
		}

		public void SetFace(Expressions incExpression, float incTime)
		{
			foreach (var expressionFace in expressionsSO.expressionFaces)
			{
				if (expressionFace.expression != incExpression) continue;

				if (hasBrows) browAnim.SetBrows(expressionFace.face.brows);
				eyesAnim.SetEyes(expressionFace.face.eyes);
				mouthAnim.SetMouth(expressionFace.face.mouth);
			}
			
			if (isPlayer)
			{
				if (incTime < 0) timeToExpress =
				UnityEngine.Random.Range(minMaxExpressionTime.x, minMaxExpressionTime.y);
				else timeToExpress = incTime;

				expressionTimer = 0;
			}
		}

		private void HandleExpressionTimer()
		{
			expressionTimer += Time.deltaTime;

			if (expressionTimer >= timeToExpress)
				SetSituationFace(ExpressionSituations.play, -1f);
		}

		private void SetNeutralFace()
		{
			SetFace(Expressions.neutral, -1);
		}

		private void SetGleefulFace()
		{
			SetFace(Expressions.gleeful, -1);
		}

		private void SetOuchFace()
		{
			SetFace(Expressions.ouch, -1);
		}

		private void SetSmileFace()
		{
			SetFace(Expressions.smiling, -1);
		}

		private void SetToothyLaughFace()
		{
			SetFace(Expressions.toothyLaugh, -1);
		}

		private void SetVeryHappyFace()
		{
			SetFace(Expressions.veryHappy, -1);
		}

		private void SetShockedFace()
		{
			SetFace(Expressions.shocked, -1);
		}

		private void SetAnnoyedFace()
		{
			SetFace(Expressions.annoyed, -1);
		}

		private void SetLookingFace()
		{
			SetFace(Expressions.looking, -1);
		}

		private void SetCalmFace()
		{
			SetFace(Expressions.calm, -1);
		}
	}
}
