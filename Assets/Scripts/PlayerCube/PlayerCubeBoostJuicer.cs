using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeBoostJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks boostJuice = null;
		[SerializeField] MMFeedbacks postBoostJuice = null;
		[SerializeField] float maxBoostTrailTime = 1f;

		//Cache
		MMFeedbackScale[] postBoostMMScalers;
		MMFeedbackPosition postBoostMMPos;
		MMFeedbackScale[] boostMMScalers;
		PlayerExpressionHandler expresHandler;

		//States
		Vector3 boostImpactDir = new Vector3(0, 0, 0);
		float boostTrailTimer = 0f;
		bool boostTrailCounting = false;

		private void Awake() 
		{
			postBoostMMScalers = postBoostJuice.GetComponents<MMFeedbackScale>();
			postBoostMMPos = postBoostJuice.GetComponent<MMFeedbackPosition>();
			boostMMScalers = boostJuice.GetComponents<MMFeedbackScale>();
			expresHandler = GetComponentInChildren<PlayerExpressionHandler>();
		}

		private void Update()
		{
			BoostTrailCounting();
		}

		private void BoostTrailCounting()
		{
			if (boostTrailCounting) boostTrailTimer += Time.deltaTime;

			if (boostTrailTimer >= maxBoostTrailTime)
			{
				boostJuice.StopFeedbacks();
				boostTrailCounting = false;
			}
		}

		public void PlayBoostJuice(Vector3 direction)
		{
			ParticleSystem particles = boostJuice.
			GetComponent<MMFeedbackParticlesInstantiation>().ParticlesPrefab;

			particles.transform.forward = transform.TransformDirection(direction);
			boostImpactDir = -direction;

			for (int i = 0; i < boostMMScalers.Length; i++)
			{
				CalculateBoostScaleAxis(i, boostMMScalers);
			}

			boostJuice.Initialization();
			boostJuice.PlayFeedbacks();
			boostTrailTimer = 0;
			boostTrailCounting = true;
			// expresHandler.SetGameplayExpression(expresHandler.
			// 	expressionsSO.boostExpressions);
		}

		public void PlayPostBoostJuice()
		{
			ResetMMPosValues();

			ParticleSystem particles = postBoostJuice.GetComponent<MMFeedbackParticlesInstantiation>().
				ParticlesPrefab;

			particles.transform.forward = transform.TransformDirection(boostImpactDir);
			postBoostJuice.GetComponent<MMFeedbackParticlesInstantiation>().Offset = boostImpactDir * .5f;

			for (int i = 0; i < postBoostMMScalers.Length; i++)
			{
				CalculateBoostScaleAxis(i, postBoostMMScalers);
			}

			CalculatePostBoostScaleMoveDir();

			boostJuice.StopFeedbacks();
			postBoostJuice.Initialization();
			postBoostJuice.PlayFeedbacks();
			// expresHandler.SetGameplayExpression(expresHandler.
			// 	expressionsSO.wallHitExpressions);
		}

		private void CalculatePostBoostScaleMoveDir()
		{
			if (V3Equal(boostImpactDir, Vector3.forward))
				SetBoostMoveValues(false, false, true, 1);

			if (V3Equal(boostImpactDir, Vector3.back))
				SetBoostMoveValues(false, false, true, -1);

			if (V3Equal(boostImpactDir, Vector3.right))
				SetBoostMoveValues(true, false, false, 1);

			if (V3Equal(boostImpactDir, Vector3.left))
				SetBoostMoveValues(true, false, false, -1);
		}

		private void CalculateBoostScaleAxis(int i, MMFeedbackScale[] scalers)
		{
			if((isBoostImpactX() && IsPlayerZWorldX()) || (isBoostsImpactZ() && IsPlayerZWorldZ()))
			{
				if (scalers[i].Label == "HeightScale")
					SetBoostScaleValues(i, scalers, false, false, true);

				if (scalers[i].Label == "WidthScale")
					SetBoostScaleValues(i, scalers, true, true, false);
			}

			if((isBoostImpactX() && IsPlayerXWorldX()) || (isBoostsImpactZ() && IsPlayerXWorldZ()))
			{
				if (scalers[i].Label == "HeightScale")
					SetBoostScaleValues(i, scalers, true, false, false);

				if (scalers[i].Label == "WidthScale")
					SetBoostScaleValues(i, scalers, false, true, true);
			}

			if ((isBoostImpactX() && IsPlayerYWorldX()) || (isBoostsImpactZ() && IsPlayerYWorldZ()))
			{
				if (scalers[i].Label == "HeightScale")
					SetBoostScaleValues(i, scalers, false, true, false);

				if (scalers[i].Label == "WidthScale")
					SetBoostScaleValues(i, scalers, true, false, true);
			}
		}

		private void SetBoostMoveValues(bool xValue, bool yValue, bool zValue, int dirValue)
		{
			postBoostMMPos.RemapCurveOne *= dirValue;

			postBoostMMPos.AnimateX = xValue;
			postBoostMMPos.AnimateY = yValue;
			postBoostMMPos.AnimateZ = zValue;
		}

		private void SetBoostScaleValues(int i, MMFeedbackScale[] scalers, 
			bool xValue, bool yValue, bool zValue)
		{
			scalers[i].AnimateX = xValue;
			scalers[i].AnimateY = yValue;
			scalers[i].AnimateZ = zValue;
		}

		private void ResetMMPosValues()
		{
			postBoostMMPos.InitialPosition = new Vector3(0, 0, 0);
			postBoostMMPos.RemapCurveOne = Mathf.Abs(postBoostMMPos.RemapCurveOne);
		}

		private bool isBoostsImpactZ()
		{
			return V3Equal(boostImpactDir, Vector3.forward) || V3Equal(boostImpactDir, Vector3.back);
		}

		private bool isBoostImpactX()
		{
			return V3Equal(boostImpactDir, Vector3.left) || V3Equal(boostImpactDir, Vector3.right);
		}

		private bool IsPlayerZWorldX()
		{
			return V3Equal(transform.forward, Vector3.left) || V3Equal(transform.forward, Vector3.right);
		}

		private bool IsPlayerXWorldX()
		{
			return V3Equal(transform.right, Vector3.left) || V3Equal(transform.right, Vector3.right);
		}

		private bool IsPlayerYWorldX()
		{
			return V3Equal(transform.up, Vector3.left) || V3Equal(transform.up, Vector3.right);
		}

		private bool IsPlayerZWorldZ()
		{
			return V3Equal(transform.forward, Vector3.forward) || V3Equal(transform.forward, Vector3.back);
		}

		private bool IsPlayerXWorldZ()
		{
			return V3Equal(transform.right, Vector3.forward) || V3Equal(transform.right, Vector3.back);
		}

		private bool IsPlayerYWorldZ()
		{
			return V3Equal(transform.up, Vector3.forward) || V3Equal(transform.up, Vector3.back);
		}

		public bool V3Equal(Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(a - b) < 0.001;
		}
	}
}

