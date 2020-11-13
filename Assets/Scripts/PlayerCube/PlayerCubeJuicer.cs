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
		[SerializeField] MMFeedbacks flipFeedback = null;
		[SerializeField] float preFlipFeedbackDuration = 0f;
		[SerializeField] MMFeedbacks postFlipFeedback = null;

		//Cache
		AudioSource source;
		MMFeedbackScale[] postFlipMMScalers = null;

		private void Awake() 
		{
			source = GetComponentInChildren<AudioSource>();
			postFlipMMScalers = postFlipFeedback.GetComponents<MMFeedbackScale>();
		}

		public void PlayPostFlipFeedbacks()
		{
			for (int i = 0; i < postFlipMMScalers.Length; i++)
			{
				CalculateScaleAxis(i);
			}

			postFlipFeedback.Initialization();
			postFlipFeedback.PlayFeedbacks();
		}

		private void CalculateScaleAxis(int i)
		{
			if (IsPlayerZWorldY())
			{
				if (postFlipMMScalers[i].Label == "HeightScale")
				{
					postFlipMMScalers[i].AnimateX = false;
					postFlipMMScalers[i].AnimateY = false;
					postFlipMMScalers[i].AnimateZ = true;
				}

				if (postFlipMMScalers[i].Label == "WidthScale")
				{
					postFlipMMScalers[i].AnimateX = true;
					postFlipMMScalers[i].AnimateY = true;
					postFlipMMScalers[i].AnimateZ = false;
				}
			}

			if (IsPlayerXWorldY())
			{

				if (postFlipMMScalers[i].Label == "HeightScale")
				{
					postFlipMMScalers[i].AnimateX = true;
					postFlipMMScalers[i].AnimateY = false;
					postFlipMMScalers[i].AnimateZ = false;
				}

				if (postFlipMMScalers[i].Label == "WidthScale")
				{
					postFlipMMScalers[i].AnimateX = false;
					postFlipMMScalers[i].AnimateY = true;
					postFlipMMScalers[i].AnimateZ = true;
				}
			}

			if (IsPlayerYWorldY())
			{
				if (postFlipMMScalers[i].Label == "HeightScale")
				{
					postFlipMMScalers[i].AnimateX = false;
					postFlipMMScalers[i].AnimateY = true;
					postFlipMMScalers[i].AnimateZ = false;
				}

				if (postFlipMMScalers[i].Label == "WidthScale")
				{
					postFlipMMScalers[i].AnimateX = true;
					postFlipMMScalers[i].AnimateY = false;
					postFlipMMScalers[i].AnimateZ = true;
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

