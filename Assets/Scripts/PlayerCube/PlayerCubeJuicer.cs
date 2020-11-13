using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip landClip = null;
		[SerializeField] MMFeedbacks flipJuice = null;
		public float preFlipJuiceDuration = 0f;
		[SerializeField] MMFeedbacks postFlipJuice = null;
		[SerializeField] MMFeedbacks boostJuice = null;
		[SerializeField] MMFeedbacks postBoostJuice = null;

		//Cache
		AudioSource source;
		MMFeedbackScale[] postFlipMMScalers;
		MMFeedbackScale[] flipMMScalers;

		//States
		Vector3 boostImpactDir = new Vector3(0, 0, 0);

		private void Awake() 
		{
			source = GetComponentInChildren<AudioSource>();
			postFlipMMScalers = postFlipJuice.GetComponents<MMFeedbackScale>();
			flipMMScalers = flipJuice.GetComponents<MMFeedbackScale>();
		}

		public void PlayFlipJuice()
		{
			for (int i = 0; i < flipMMScalers.Length; i++)
			{
				CalculateScaleAxis(i, flipMMScalers);
			}

			flipJuice.Initialization();
			flipJuice.PlayFeedbacks();
		}

		public void PlayPostFlipJuice()
		{
			for (int i = 0; i < postFlipMMScalers.Length; i++)
			{
				CalculateScaleAxis(i, postFlipMMScalers);
			}

			postFlipJuice.Initialization();
			postFlipJuice.PlayFeedbacks();
		}

		public void PlayBoostJuice(Vector3 direction)
		{
			ParticleSystem particles = boostJuice.GetComponent<MMFeedbackParticlesInstantiation>().
				ParticlesPrefab;

			particles.transform.forward = transform.TransformDirection(direction);

			boostImpactDir = -direction;
			
			boostJuice.Initialization();
			boostJuice.PlayFeedbacks();
		}

		public void PlayPostBoostJuice()
		{
			ParticleSystem particles = postBoostJuice.GetComponent<MMFeedbackParticlesInstantiation>().
				ParticlesPrefab;

			particles.transform.forward = transform.TransformDirection(boostImpactDir);
			postBoostJuice.GetComponent<MMFeedbackParticlesInstantiation>().Offset = boostImpactDir * .5f;

			boostJuice.StopFeedbacks();
			postBoostJuice.Initialization();
			postBoostJuice.PlayFeedbacks();
		}

		private void CalculateScaleAxis(int i, MMFeedbackScale[] scalers)
		{
			if (IsPlayerZWorldY())
			{
				if (scalers[i].Label == "HeightScale")
				{
					scalers[i].AnimateX = false;
					scalers[i].AnimateY = false;
					scalers[i].AnimateZ = true;
				}

				if (scalers[i].Label == "WidthScale")
				{
					scalers[i].AnimateX = true;
					scalers[i].AnimateY = true;
					scalers[i].AnimateZ = false;
				}
			}

			if (IsPlayerXWorldY())
			{

				if (scalers[i].Label == "HeightScale")
				{
					scalers[i].AnimateX = true;
					scalers[i].AnimateY = false;
					scalers[i].AnimateZ = false;
				}

				if (scalers[i].Label == "WidthScale")
				{
					scalers[i].AnimateX = false;
					scalers[i].AnimateY = true;
					scalers[i].AnimateZ = true;
				}
			}

			if (IsPlayerYWorldY())
			{
				if (scalers[i].Label == "HeightScale")
				{
					scalers[i].AnimateX = false;
					scalers[i].AnimateY = true;
					scalers[i].AnimateZ = false;
				}

				if (scalers[i].Label == "WidthScale")
				{
					scalers[i].AnimateX = true;
					scalers[i].AnimateY = false;
					scalers[i].AnimateZ = true;
				}
			}
		}

		private bool IsPlayerZWorldY()
		{
			return transform.forward == new Vector3(0, 1, 0) || transform.forward == new Vector3(0, -1, 0);
		}

		private bool IsPlayerXWorldY()
		{
			return transform.right == new Vector3(0, 1, 0) || transform.right == new Vector3(0, -1, 0);
		}


		private bool IsPlayerYWorldY()
		{
			return transform.up == new Vector3(0, 1, 0) || transform.up == new Vector3(0, -1, 0);
		}

		public void PlayLandClip()
		{
			AudioSource.PlayClipAtPoint(landClip, Camera.main.transform.position, .2f);
		}

	}
}

