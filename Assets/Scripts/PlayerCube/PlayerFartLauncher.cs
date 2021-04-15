using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerFartLauncher : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float[] fartForces;
		[SerializeField] float[] intervals;
		[SerializeField] float fallMultiplier = 2.5f;
		[SerializeField] float lowLaunchMultiplier = 2f;
		[SerializeField] float stopFartViewportPos = 1.2f;
		[SerializeField] MMFeedbacks preFartJuice = null;

		//Cache
		GameControls controls;
		Rigidbody rb;
		MMFeedbackWiggle preFartMMWiggle;

		//States
		Vector3 viewPos;

		//Actions, events, delegates etc
		public event Action onDoneFarting;

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			preFartMMWiggle = preFartJuice.GetComponent<MMFeedbackWiggle>();
		}

		private void Update() 
		{
			viewPos = Camera.main.WorldToViewportPoint(transform.position);
			AddGravityIfFalling();	
		}

		public void InitiateFartSequence()
		{
			StartCoroutine(FartSequence());
		}

		private IEnumerator FartSequence()
		{
			transform.parent = null;
			
			print("rumble");
			preFartJuice.Initialization();
			preFartJuice.PlayFeedbacks();

			float feedbackDuration = preFartMMWiggle.WigglePositionDuration;
			yield return new WaitForSeconds(feedbackDuration);
			print("done rumbling");

			rb.isKinematic = false;

			while (viewPos.y < stopFartViewportPos)
			{
				print("farting");
				int forceIndex = UnityEngine.Random.Range(0, fartForces.Length);
				LaunchPlayer(fartForces[forceIndex]);
				
				int intervalIndex = 0;
				if (forceIndex <= 1) intervalIndex = UnityEngine.Random.Range(0, 1);
				else if ( forceIndex > 1) intervalIndex = UnityEngine.Random.Range(1, intervals.Length);

				yield return new WaitForSeconds(intervals[intervalIndex]);
			}
			print("done farting");
			rb.isKinematic = true;
			onDoneFarting();
		}

		private void LaunchPlayer(float force)
		{
			rb.velocity = Vector3.up * force;
		}

		private void AddGravityIfFalling()
		{
			if (rb.velocity.y < 0)
			{
				rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
			}
			else if (rb.velocity.y > 0)
			{
				rb.velocity += Vector3.up * Physics.gravity.y * (lowLaunchMultiplier - 1) * Time.deltaTime;
			}
		}
	}
}

