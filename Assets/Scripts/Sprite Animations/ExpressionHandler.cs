using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.SpriteAnimations
{
	public class ExpressionHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] ExpressionsScripOb expressionsSO;
		[SerializeField] SituationsExprsScripOb situationsExprsSO;
		[SerializeField] bool isPlayer;
		[SerializeField] bool hasBrows = true, hasMouth = true;
		[SerializeField] Vector2 minMaxExpressionTime, minMaxBlinkTime;
		[SerializeField] float blinkDur = .2f;

		//Cache
		SpriteBrowAnimator browAnim = null;
		SpriteEyesAnimator eyesAnim = null;
		SpriteMouthAnimator mouthAnim = null;
		FaceJuicer faceJuice = null;

		//States
		float expressionTimer = 0f, blinkTimer = 0f;
		float timeToExpress = 0f, timeToBlink = 0f;
		public bool hasFinished { get; set; } = false;
		bool isIdling = true, canBlink = true;

		//Actions, events, delegates etc
		public Func<bool> onFetchStunned;
		// Func works same as delegates but shorter. The last parameter 
		// is always the return type. Any in front of that are just paramaters

		private void Awake()
		{
			browAnim = GetComponentInChildren<SpriteBrowAnimator>();
			eyesAnim = GetComponentInChildren<SpriteEyesAnimator>();
			mouthAnim = GetComponentInChildren<SpriteMouthAnimator>();
			faceJuice = GetComponent<FaceJuicer>();
		}

		private void Update()
		{
			if (isPlayer && !hasFinished)
			{
				HandleBlinkTimer();
				HandleExpressionTimer(); 
			}
		}

		public void SetSituationFace(ExpressionSituations incSituation, float incTime)
		{
			if (isPlayer)
			{
				if (incSituation == ExpressionSituations.play) isIdling = true;
				else isIdling = false;

				foreach (var situationExpr in situationsExprsSO.situationExpressions)
				{
					if (situationExpr.situation != incSituation) continue;

					int index = UnityEngine.Random.Range(0, situationExpr.expressions.Length);
					var expressionToSet = situationExpr.expressions[index];

					SetFace(expressionToSet, incTime);
				}
			}
		}

		public float GetRandomTime()
		{
			return UnityEngine.Random.Range(minMaxExpressionTime.x, minMaxExpressionTime.y);
		}

		public void SetFace(Expressions incExpression, float incTime)
		{
			// smiling is currently the only idle face that doesn't have it's own eye movement
			// so only in that face can we activate blink
			if (incExpression == Expressions.smiling) canBlink = true;
			else canBlink = false;
			
			foreach (var expressionFace in expressionsSO.expressionFaces)
			{
				if (expressionFace.expression != incExpression) continue;

				if (hasBrows) browAnim.SetBrows(expressionFace.face.brows);
				eyesAnim.SetEyes(expressionFace.face.eyes);
				if (hasMouth) mouthAnim.SetMouth(expressionFace.face.mouth);
			}
			
			if (isPlayer)
			{
				timeToExpress = incTime;
				expressionTimer = 0;
			}
		}

		private void InitiateBlinking() //In case blink is called from animations
		{
			StartCoroutine(Blink());
		}

		//Can only do this from idle states that use neutral eyes bc it will
		//always go back to neutral
		private IEnumerator Blink()
		{
			if (isIdling && canBlink)
			{
				foreach (var expressionFace in expressionsSO.expressionFaces)
				{
					if (expressionFace.expression == Expressions.blink)
						eyesAnim.SetEyes(expressionFace.face.eyes);
				}

				yield return new WaitForSeconds(blinkDur);

				foreach (var expressionFace in expressionsSO.expressionFaces)
				{
					if (expressionFace.expression == Expressions.neutral)
						eyesAnim.SetEyes(expressionFace.face.eyes);
				}
			}
		}

		private void HandleBlinkTimer()
		{
			blinkTimer += Time.deltaTime;
			if (blinkTimer >= timeToBlink)
			{
				StartCoroutine(Blink());
				timeToBlink = UnityEngine.Random.Range(minMaxBlinkTime.x, minMaxBlinkTime.y);
				blinkTimer = 0;
			}
		}

		private void HandleExpressionTimer()
		{
			expressionTimer += Time.deltaTime;

			if (expressionTimer >= timeToExpress)
			{
				if (!onFetchStunned())
					SetSituationFace(ExpressionSituations.play, GetRandomTime());
				else SetSituationFace(ExpressionSituations.laserHit, GetRandomTime());
			}
		}

		private void SetNeutralFace()
		{
			SetFace(Expressions.neutral, GetRandomTime());
		}

		private void SetGleefulFace()
		{
			SetFace(Expressions.gleeful, GetRandomTime());
		}

		private void SetOuchFace()
		{
			SetFace(Expressions.ouch, GetRandomTime());
		}

		private void SetSmileFace()
		{
			SetFace(Expressions.smiling, GetRandomTime());
		}

		private void SetToothyLaughFace()
		{
			SetFace(Expressions.toothyLaugh, GetRandomTime());
		}

		private void SetVeryHappyFace()
		{
			SetFace(Expressions.veryHappy, GetRandomTime());
		}

		private void SetShockedFace()
		{
			SetFace(Expressions.shocked, GetRandomTime());
		}

		private void SetAnnoyedFace()
		{
			SetFace(Expressions.annoyed, GetRandomTime());
		}

		private void SetLookingFace()
		{
			SetFace(Expressions.looking, GetRandomTime());
		}

		private void SetCalmFace()
		{
			SetFace(Expressions.calm, GetRandomTime());
		}

		public void SetLaughingFace()
        {
			SetFace(Expressions.laughing, GetRandomTime());
        }

		private void StartLaughingWiggle()
		{
			faceJuice.WiggleFace();
		}
	}
}
