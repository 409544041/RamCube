using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeFlipJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks flipJuice = null;
		public float preFlipJuiceDuration = 0f;
		[SerializeField] MMFeedbacks postFlipJuice = null;

		//Cache
		MMFeedbackScale[] postFlipMMScalers;
		MMFeedbackScale[] flipMMScalers;
		MMFeedbackPosition postFlipMMPos;
		ExpressionHandler expresHandler;
		ParticleSystem postFlipParticles;

		//States
		float postFlipCurveOneAtStart = 0;

		private void Awake() 
		{
			postFlipMMScalers = postFlipJuice.GetComponents<MMFeedbackScale>();
			flipMMScalers = flipJuice.GetComponents<MMFeedbackScale>();
			postFlipMMPos = postFlipJuice.GetComponent<MMFeedbackPosition>();
			expresHandler = GetComponentInChildren<ExpressionHandler>();
			postFlipParticles = postFlipJuice.GetComponent<MMFeedbackParticles>().BoundParticleSystem;
		}

		private void Start() 
		{
			postFlipCurveOneAtStart = postFlipMMPos.RemapCurveOne;
			postFlipJuice.Initialization();
		}

		public void PlayFlipJuice()
		{
			for (int i = 0; i < flipMMScalers.Length; i++)
			{
				CalculateFlipScaleAxis(i, flipMMScalers);
			}

			flipJuice.Initialization();
			flipJuice.PlayFeedbacks();
		}

		public void PlayPostFlipJuice()
		{
			for (int i = 0; i < postFlipMMScalers.Length; i++)
			{
				CalculateFlipScaleAxis(i, postFlipMMScalers);
			}

			CalculateScaleReposDir();
			SetParticle();

			postFlipJuice.PlayFeedbacks();

			expresHandler.SetSituationFace(ExpressionSituations.flip, .5f);
		}

		private void CalculateFlipScaleAxis(int i, MMFeedbackScale[] scalers)
		{

			if (IsPlayerZWorldY())
			{
				if (scalers[i].Label == "HeightScale")
					SetFlipScaleValues(i, scalers, false, false, true);

				if (scalers[i].Label == "WidthScale")
					SetFlipScaleValues(i, scalers, true, true, false);
			}

			if (IsPlayerXWorldY())
			{
				if (scalers[i].Label == "HeightScale")
					SetFlipScaleValues(i, scalers, true, false, false);

				if (scalers[i].Label == "WidthScale")
					SetFlipScaleValues(i, scalers, false, true, true);
			}

			if (IsPlayerYWorldY())
			{
				if (scalers[i].Label == "HeightScale")
					SetFlipScaleValues(i, scalers, false, true, false);

				if (scalers[i].Label == "WidthScale")
					SetFlipScaleValues(i, scalers, true, false, true);
			}
		}

		private void SetParticle()
		{
			postFlipParticles.transform.forward = Vector3.up;
			postFlipParticles.transform.position = new Vector3
				(transform.position.x, .5f, transform.position.z);
		}

		//below needs to be done bc we're repositioning this MMJuice local and not world
		private void CalculateScaleReposDir()
		{
			if (V3Equal(transform.forward, Vector3.up))
				SetScaleReposValues(false, false, true, 1);

			if (V3Equal(-transform.forward, Vector3.up))
				SetScaleReposValues(false, false, true, -1);

			if (V3Equal(transform.up, Vector3.up))
				SetScaleReposValues(false, true, false, 1);

			if (V3Equal(-transform.up, Vector3.up))
				SetScaleReposValues(false, true, false, -1);

			if (V3Equal(transform.right, Vector3.up))
				SetScaleReposValues(true, false, false, 1);

			if (V3Equal(-transform.right, Vector3.up))
				SetScaleReposValues(true, false, false, -1);
		}

		private void SetFlipScaleValues(int i, MMFeedbackScale[] scalers,
			bool xValue, bool yValue, bool zValue)
		{
			scalers[i].AnimateX = xValue;
			scalers[i].AnimateY = yValue;
			scalers[i].AnimateZ = zValue;
		}

		private void SetScaleReposValues(bool xValue, bool yValue, bool zValue, int curveMult)
		{
			postFlipMMPos.RemapCurveOne = postFlipCurveOneAtStart;
			postFlipMMPos.RemapCurveOne *= curveMult;
			postFlipMMPos.AnimateX = xValue;
			postFlipMMPos.AnimateY = yValue;
			postFlipMMPos.AnimateZ = zValue;
		}

		private bool IsPlayerZWorldY()
		{
			return V3Equal(transform.forward, Vector3.up) || V3Equal(transform.forward, Vector3.down);
		}

		private bool IsPlayerXWorldY()
		{
			return V3Equal(transform.right, Vector3.up) || V3Equal(transform.right, Vector3.down);
		}

		private bool IsPlayerYWorldY()
		{
			return V3Equal(transform.up, Vector3.up) || V3Equal(transform.up, Vector3.down);
		}

		private bool V3Equal(Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(a - b) < 0.001;
		}
	}
}

