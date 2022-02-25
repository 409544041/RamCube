using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Peep
{
	public class PeepExpressionHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] PeepRefHolder refs;
		[SerializeField] Vector2 minMaxBlinkInterval;
		[SerializeField] PeepExpression[] peepEyeExpressions;
		[SerializeField] float shockSignalTime = .5f, questionSignalTime = 1, 
			questionSignalDelay = .5f, shockSignalDelay = .25f;

		[System.Serializable]
		public class PeepExpression
		{
			public Expressions expression;
			public Texture2D eyeTex;
		}

		//States
		bool canBlink = true;
		float blinkTimer = 0;
		float timeToBlink = 0;
		public PeepExpression currentExpr { get; private set; }
		public PeepExpression neutralExpr { get; private set; }
		public PeepExpression annoyedExpr { get; private set; }
		public PeepExpression angryExpr { get; private set; }
		public PeepExpression smilingExpr { get; private set; }
		public PeepExpression blinkExpr { get; private set; }

		private void Awake()
		{
			foreach (var expr in peepEyeExpressions)
			{
				if (expr.expression == Expressions.neutral) neutralExpr = expr;
				if (expr.expression == Expressions.annoyed) annoyedExpr = expr;
				if (expr.expression == Expressions.angry) angryExpr = expr;
				if (expr.expression == Expressions.smiling) smilingExpr = expr;
				if (expr.expression == Expressions.blink) blinkExpr = expr;
			}

			currentExpr = neutralExpr;
		}

		private void Start()
		{
			timeToBlink = Random.Range(minMaxBlinkInterval.x, minMaxBlinkInterval.y);
		}

		private void Update()
		{
			if (canBlink) HandleBlinkTimer();
		}

		private void HandleBlinkTimer()
		{
			blinkTimer += Time.deltaTime;
			if (blinkTimer >= timeToBlink) StartCoroutine(Blink());
		}

		private IEnumerator Blink()
		{
			blinkTimer = 0;

			SetExpression(annoyedExpr);
			yield return null;
			yield return null;
			SetExpression(blinkExpr);
			yield return null;
			yield return null;
			SetExpression(annoyedExpr);
			yield return null;
			yield return null;
			SetExpression(currentExpr);
		}

		private void SetExpression(PeepExpression expression)
		{
			refs.meshes[2].material.mainTexture = expression.eyeTex;
		}

		public void SetNeutralExpression() 
		{
			SetExpression(neutralExpr);
			currentExpr = neutralExpr;
		}

		public void SetAnnoyedExpression() 
		{
			SetExpression(annoyedExpr);
			currentExpr = annoyedExpr;
		}

		public void SetShockExprSignal()
		{
			StartCoroutine(HandleShockExprSignal());
		}

		private IEnumerator HandleShockExprSignal()
		{
			yield return new WaitForSeconds(shockSignalDelay);
			refs.shockUIJuice.Initialization();
			refs.shockUIJuice.PlayFeedbacks();
			refs.expressionSignals[0].alpha = 1;
			yield return new WaitForSeconds(shockSignalTime);
			refs.expressionSignals[0].alpha = 0;
		}

		public void SetQuestionExprSignal()
		{
			StartCoroutine(HandleQuestionExprSignal());
		}

		private IEnumerator HandleQuestionExprSignal()
		{
			yield return new WaitForSeconds(questionSignalDelay);
			refs.questionUIjuice.Initialization();
			refs.questionUIjuice.PlayFeedbacks();
			refs.expressionSignals[1].alpha = 1;
			yield return new WaitForSeconds(questionSignalTime);
			refs.expressionSignals[1].alpha = 0;
		}

		private void OnDisable()
		{
			refs.meshes[2].material.mainTexture = neutralExpr.eyeTex;
		}
	}
}
