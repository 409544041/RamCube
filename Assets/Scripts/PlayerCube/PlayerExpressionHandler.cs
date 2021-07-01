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

		private void Awake()
		{
			browAnim = GetComponentInChildren<PlayerSpriteBrowAnimator>();
			eyesAnim = GetComponentInChildren<PlayerSpriteEyesAnimator>();
			mouthAnim = GetComponentInChildren<PlayerSpriteMouthAnimator>();
		}

		public void SetFace(ExpressionSituations incSituation)
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
		}
	}
}
