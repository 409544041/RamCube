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

		//Cache
		MMFeedbackScale[] postBoostMMScalers;
		MMFeedbackPosition postBoostMMPos;
		MMFeedbackScale[] boostMMScalers;

		//States
		Vector3 boostImpactDir = new Vector3(0, 0, 0);

		private void Awake() 
		{
			postBoostMMScalers = postBoostJuice.GetComponents<MMFeedbackScale>();
			postBoostMMPos = postBoostJuice.GetComponent<MMFeedbackPosition>();
			boostMMScalers = boostJuice.GetComponents<MMFeedbackScale>();
		}

		public void PlayBoostJuice(Vector3 direction)
		{
			ParticleSystem particles = boostJuice.GetComponent<MMFeedbackParticlesInstantiation>().
				ParticlesPrefab;

			particles.transform.forward = transform.TransformDirection(direction);
			boostImpactDir = -direction;

			for (int i = 0; i < boostMMScalers.Length; i++)
			{
				CalculateBoostScaleAxis(i, boostMMScalers);
			}

			boostJuice.Initialization();
			boostJuice.PlayFeedbacks();
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
		}

		private void CalculatePostBoostScaleMoveDir()
		{
			if (boostImpactDir == new Vector3(0, 0, 1))
				SetBoostMoveValues(false, false, true, 1);

			if (boostImpactDir == new Vector3(0, 0, -1))
				SetBoostMoveValues(false, false, true, -1);

			if (boostImpactDir == new Vector3(1, 0, 0))
				SetBoostMoveValues(true, false, false, 1);

			if (boostImpactDir == new Vector3(-1, 0, 0))
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
			return boostImpactDir == Vector3.forward || boostImpactDir == Vector3.back;
		}

		private bool isBoostImpactX()
		{
			return boostImpactDir == Vector3.left || boostImpactDir == Vector3.right;
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
			return Vector3.SqrMagnitude(a - b) < 0.0001;
		}
	}
}

