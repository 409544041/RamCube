using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeFlipJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip landClip = null;
		[SerializeField] MMFeedbacks flipJuice = null;
		public float preFlipJuiceDuration = 0f;
		[SerializeField] MMFeedbacks postFlipJuice = null;

		//Cache
		AudioSource source;
		MMFeedbackScale[] postFlipMMScalers;
		MMFeedbackScale[] flipMMScalers;

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

			postFlipJuice.Initialization();
			postFlipJuice.PlayFeedbacks();
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

		private void SetFlipScaleValues(int i, MMFeedbackScale[] scalers,
			bool xValue, bool yValue, bool zValue)
		{
			scalers[i].AnimateX = xValue;
			scalers[i].AnimateY = yValue;
			scalers[i].AnimateZ = zValue;
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

		public bool V3Equal(Vector3 a, Vector3 b)
		{
			return Vector3.SqrMagnitude(a - b) < 0.0001;
		}

		public void PlayLandClip()
		{
			AudioSource.PlayClipAtPoint(landClip, Camera.main.transform.position, .2f);
		}

	}
}

