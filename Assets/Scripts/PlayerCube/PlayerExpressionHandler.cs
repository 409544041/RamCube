using System.Collections;
using System.Collections.Generic;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerExpressionHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Expression[] expressions;
		[SerializeField] Vector2 minMaxExpressionTime;

		[System.Serializable]
		public class Expression
		{
			public ExpressionSituations situation;
			public FacialState[] facialStates;
		}

		[System.Serializable]
		public class FacialState
		{
			public BrowStates brows;
			public EyesStates eyes;
			public MouthStates mouth;
		}

		//Cache
		PlayerSpriteBrowAnimator browAnim = null;
		PlayerSpriteEyesAnimator eyesAnim = null;
		PlayerSpriteMouthAnimator mouthAnim = null;
		PlayerCubeMover mover = null;

		//States
		float expressionTimer = 0f;
		float timeToExpress = 0f;

		private void Awake()
		{
			browAnim = GetComponentInChildren<PlayerSpriteBrowAnimator>();
			eyesAnim = GetComponentInChildren<PlayerSpriteEyesAnimator>();
			mouthAnim = GetComponentInChildren<PlayerSpriteMouthAnimator>();
			mover = GetComponentInParent<PlayerCubeMover>();
		}

		private void Update()
		{
			HandleExpressionTimer();
		}

		public void SetFace(ExpressionSituations incSituation, float incTime)
		{
			foreach (var expression in expressions)
			{
				if (expression.situation != incSituation) continue;

				int index = Random.Range(0, expression.facialStates.Length -1);
				var expressionToSet = expression.facialStates[index];

				browAnim.SetBrows(expressionToSet.brows);
				eyesAnim.SetEyes(expressionToSet.eyes);
				mouthAnim.SetMouth(expressionToSet.mouth);
			}

			if (incTime < 0) timeToExpress = 
				Random.Range(minMaxExpressionTime.x, minMaxExpressionTime.y);
			else timeToExpress = incTime;
			
			expressionTimer = 0;
		}

		private void HandleExpressionTimer()
		{
			expressionTimer += Time.deltaTime;

			if (expressionTimer >= timeToExpress)
				SetFace(ExpressionSituations.play, -1f);
		}
	}
}
