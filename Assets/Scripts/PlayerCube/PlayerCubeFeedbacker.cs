using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeFeedbacker : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip landClip = null;
		[SerializeField] MMFeedbacks flipFeedback;
		[SerializeField] float preFlipFeedbackDuration;
		[SerializeField] MMFeedbacks postFlipFeedback;

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
			if (IsForwardY())
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

			if ((isForwardZ() && isUpX()) || (isForwardX() && isUpZ()))
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

			if ((isUpY() && isForwardZ()) || (isUpY() && isForwardX()))
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

		private bool IsForwardY()
		{
			return transform.forward == new Vector3(0, 1, 0) || transform.forward == new Vector3(0, -1, 0);
		}

		private bool isForwardX()
		{
			return transform.forward == new Vector3(1, 0, 0) || transform.forward == new Vector3(-1, 0, 0);
		}

		private bool isForwardZ()
		{
			return transform.forward == new Vector3(0, 0, 1) || transform.forward == new Vector3(0, 0, -1);
		}

		private bool isUpY()
		{
			return transform.up == new Vector3(0, 1, 0) || transform.up == new Vector3(0, -1, 0);
		}

		private bool isUpX()
		{
			return transform.up == new Vector3(1, 0, 0) || transform.up == new Vector3(-1, 0, 0);
		}

		private bool isUpZ()
		{
			return transform.up == new Vector3(0, 0, 1) || transform.up == new Vector3(0, 0, -1);
		}

		public void PlayLandClip()
		{
			AudioSource.PlayClipAtPoint(landClip, Camera.main.transform.position, .2f);
		}

	}
}

