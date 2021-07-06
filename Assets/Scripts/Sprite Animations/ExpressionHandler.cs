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

		private void Awake()
		{
			browAnim = GetComponentInChildren<SpriteBrowAnimator>();
			eyesAnim = GetComponentInChildren<SpriteEyesAnimator>();
			mouthAnim = GetComponentInChildren<SpriteMouthAnimator>();
		}

		private void Update()
		{
			if (isPlayer) HandleExpressionTimer();
		}

		public void SetFace(ExpressionSituations incSituation, float incTime)
		{
			print("Setting Face " + incSituation);
			foreach (var expression in expressionsSO.expressions)
			{
				if (expression.situation != incSituation) continue;

				int index = Random.Range(0, expression.facialStates.Length);
				var expressionToSet = expression.facialStates[index];

				if (hasBrows) browAnim.SetBrows(expressionToSet.brows);
				eyesAnim.SetEyes(expressionToSet.eyes);
				mouthAnim.SetMouth(expressionToSet.mouth);
			}
			
			if (isPlayer)
			{
				if (incTime < 0) timeToExpress =
				Random.Range(minMaxExpressionTime.x, minMaxExpressionTime.y);
				else timeToExpress = incTime;

				expressionTimer = 0;
			}
		}

		private void HandleExpressionTimer()
		{
			expressionTimer += Time.deltaTime;

			if (expressionTimer >= timeToExpress)
				SetFace(ExpressionSituations.play, -1f);
		}
	}
}
