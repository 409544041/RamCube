using Qbism.Cubes;
using Qbism.Environment;
using Qbism.PlayerCube;
using Qbism.Serpent;
using Qbism.Shapies;
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
		[SerializeField] bool hasBrows = true, hasMouth = true;
		[SerializeField] Vector2 minMaxExpressionTime, minMaxBlinkTime;
		[SerializeField] float blinkDur = .2f;
		[SerializeField] SpriteBrowAnimator browAnim;
		[SerializeField] SpriteEyesAnimator eyesAnim;
		[SerializeField] SpriteMouthAnimator mouthAnim;
		[SerializeField] FaceJuicer faceJuice;
		[SerializeField] SegmentRefHolder segRef;
		[SerializeField] PlayerRefHolder pRef;
		[SerializeField] ShapieRefHolder sRef;
		[SerializeField] WallRefHolder wRef;

		//States
		float expressionTimer = 0f, blinkTimer = 0f;
		float timeToExpress = 0f, timeToBlink = 0f;
		public bool hasFinished { get; set; } = false;
		bool isIdlingInGame = true, canBlink = true;
		bool inSerpScreen, pauzeExpressionTimer = false;
		ScreenStates currentScreenState;

		//Actions, events, delegates etc
		public Func<bool> onFetchStunned;

		private void Start()
		{
			if (segRef != null && segRef.scRef != null) inSerpScreen = true;
			if (inSerpScreen) SetFace(Expressions.smiling, GetRandomTime());
		}

		private void Update()
		{
			if (wRef != null) return;

			GetCurrentScreenState();

			if ((pRef != null && !hasFinished) || sRef != null ||
				(inSerpScreen && currentScreenState != ScreenStates.dialogueOverlayState))
			{
				HandleBlinkTimer();
				HandleExpressionTimer();
			}
		}

		public void SetSituationFace(ExpressionSituations incSituation, float incTime)
		{
			if (pRef != null || sRef != null ||
				(inSerpScreen && currentScreenState != ScreenStates.dialogueOverlayState))
			{
				if (incSituation == ExpressionSituations.play) isIdlingInGame = true;
				else isIdlingInGame = false;

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
			foreach (var expressionFace in expressionsSO.expressionFaces)
			{
				if (expressionFace.expression != incExpression) continue;

				canBlink = expressionFace.face.canBlink;

				if (hasBrows) browAnim.SetBrows(expressionFace.face.brows);
				eyesAnim.SetEyes(expressionFace.face.eyes);
				if (hasMouth) mouthAnim.SetMouth(expressionFace.face.mouth);
			}

			if (pRef != null || sRef != null || (inSerpScreen &&
				currentScreenState != ScreenStates.dialogueOverlayState))
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
			if ((isIdlingInGame || sRef != null || (inSerpScreen &&
				currentScreenState != ScreenStates.dialogueOverlayState)) && canBlink)
			{
				pauzeExpressionTimer = true;

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

				pauzeExpressionTimer = false;
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
			if (pauzeExpressionTimer) return;

			expressionTimer += Time.deltaTime;

			if (expressionTimer < timeToExpress) return;

			if (pRef != null)
			{
				if (!onFetchStunned())
					SetSituationFace(ExpressionSituations.play, GetRandomTime());
				else SetSituationFace(ExpressionSituations.laserHit, GetRandomTime());
			}

			if (sRef == null && inSerpScreen && currentScreenState != ScreenStates.dialogueOverlayState)
				SetSituationFace(ExpressionSituations.idle, GetRandomTime());
		}

		private void GetCurrentScreenState()
		{
			if (sRef != null) return;
			if (segRef != null)
			{
				if (segRef.gcRef != null)
					currentScreenState = segRef.gcRef.glRef.screenStateMngr.currentStateEnum;
				else if (segRef.mcRef != null)
					currentScreenState = segRef.mcRef.mlRef.screenStateMngr.currentStateEnum;
				else if (segRef.scRef != null)
					currentScreenState = segRef.scRef.slRef.screenStateMngr.currentStateEnum;
			}
			else currentScreenState = pRef.gcRef.glRef.screenStateMngr.currentStateEnum;
		}

		private void SetNeutralFace() //Called from animation
		{
			SetFace(Expressions.neutral, GetRandomTime());
		}

		private void SetGleefulFace() //Called from animation
		{
			SetFace(Expressions.gleeful, GetRandomTime());
		}

		private void SetOuchFace() //Called from animation
		{
			SetFace(Expressions.ouch, GetRandomTime());
		}

		private void SetSmileFace() //Called from animation
		{
			SetFace(Expressions.smiling, GetRandomTime());
		}

		private void SetToothyLaughFace() //Called from animation
		{
			SetFace(Expressions.toothyLaugh, GetRandomTime());
		}

		private void SetVeryHappyFace() //Called from animation
		{
			SetFace(Expressions.veryHappy, GetRandomTime());
		}

		private void SetShockedFace() //Called from animation
		{
			SetFace(Expressions.shocked, GetRandomTime());
		}

		private void SetAnnoyedFace() //Called from animation
		{
			SetFace(Expressions.annoyed, GetRandomTime());
		}

		private void SetLookingFace() //Called from animation
		{
			SetFace(Expressions.looking, GetRandomTime());
		}

		private void SetCalmFace() //Called from animation
		{
			SetFace(Expressions.calm, GetRandomTime());
		}

		public void SetLaughingFace() //Called from animation
		{
			SetFace(Expressions.laughing, GetRandomTime());
		}

		public void StartLaughingWiggle() //Called from animation
		{
			SetFace(Expressions.gleeful, GetRandomTime());
			faceJuice.WiggleFace(2);
		}

		public void StartSerpentLaughWiggle()
		{
			SetFace(Expressions.gleeful, GetRandomTime());
			faceJuice.WiggleFace(1);
		}
	}
}
