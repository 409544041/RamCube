using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerFartLauncher : MonoBehaviour
	{
		//Config parameters
		[Header ("Farts")]
		// [SerializeField] float[] fartForces;
		// [SerializeField] float[] intervals;
		[SerializeField] float fartForce;
		[SerializeField] float stopFartViewportPos = 1.2f;
		[SerializeField] LayerMask fartRayCastLayers;
		[SerializeField] Transform fartVFXParent;
		[SerializeField] float impactBuffer;
		[Header("Falling")]
		[SerializeField] float fallMultiplier = 2.5f;
		[SerializeField] float lowLaunchMultiplier = 2f;
		[Header ("Juice")]
		[SerializeField] MMFeedbacks preFartJuice = null;
		[SerializeField] ParticleSystem fartCharge, bulletFart, fartBeam, 
			fartBeamImpact, bulletFartImpact;
		[SerializeField] GameObject playerVis;

		//Cache
		GameControls controls;
		Rigidbody rb;
		MMFeedbackWiggle preFartMMWiggle;

		//States
		Vector3 viewPos;
		bool doneFarting = false;
		bool fartCollided = false;
		bool fartCounting = false;
		int fartCount = 0;
		ParticleSystem currentFartImpact = null;

		//Actions, events, delegates etc
		public event Action onDoneFarting;
		public event Action onStartFarting;
		public event Action onSwitchToEndCam;

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			preFartMMWiggle = preFartJuice.GetComponent<MMFeedbackWiggle>();
		}

		private void Update() 
		{
			if(fartCounting) fartCount++;
			if(fartCount > 10) StopFartHit();

			viewPos = Camera.main.WorldToViewportPoint(transform.position);

			// AddGravityIfFalling();
		}

		public void InitiateFartSequence(Transform target)
		{
			StartCoroutine(FartSequence(target));
		}

		private IEnumerator FartSequence(Transform target)
		{
			transform.parent = null;
			
			preFartJuice.Initialization();
			preFartJuice.PlayFeedbacks();
			fartCharge.Play();

			float feedbackDuration = preFartMMWiggle.WigglePositionDuration;
			yield return new WaitForSeconds(feedbackDuration);

			rb.isKinematic = false;

			// while (viewPos.y < stopFartViewportPos)
			// {
			// 	int forceIndex = UnityEngine.Random.Range(0, fartForces.Length);
			// 	LaunchPlayer(fartForces[forceIndex]);
				
			// 	int intervalIndex = 0;
			// 	if (forceIndex <= 1) intervalIndex = UnityEngine.Random.Range(0, 1);
			// 	else if ( forceIndex > 1) intervalIndex = UnityEngine.Random.Range(1, intervals.Length);

			// 	yield return new WaitForSeconds(intervals[intervalIndex]);
			// }

			fartBeam.Play();
			onStartFarting();
			StartCoroutine(LaunchPlayer(target));
		}

		public void StartBeamImpact()
		{
			currentFartImpact = fartBeamImpact;
			ProcessFartRaycast();
		}

		public void StartLaserBulletImpact()
		{
			currentFartImpact = bulletFartImpact;
			ProcessFartRaycast();
		}

		private void ProcessFartRaycast()
		{
			bool hasHit = false;
			RaycastHit hit = FireRayCast(out hasHit);
			
			if (hasHit)
			{
				if(!fartCollided)
				{
					currentFartImpact.transform.parent = null;
					currentFartImpact.transform.position = Vector3.Lerp(hit.point, transform.position, .05f);
					currentFartImpact.Play();
					fartCollided = true;
				}

				fartCount = 0;
				fartCounting = true;
			}
		}

		public RaycastHit FireRayCast(out bool hasHit)
		{
			//returning hasHit bool for a raycast check done in laser cube
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 20, 
				fartRayCastLayers, QueryTriggerInteraction.Ignore))
				hasHit = true;

			else hasHit = false;

			return hit;
		}

		public void FireBulletFart()
		{
			bulletFart.Play();
			GetComponentInChildren<ExpressionHandler>().
				SetFace(ExpressionSituations.fart, .75f);
		}

		private void StopFartHit()
		{
			var module = currentFartImpact.main;
			if (module.loop) currentFartImpact.Stop();
			currentFartImpact.transform.parent = fartVFXParent;
			fartCounting = false;
		}

		private IEnumerator LaunchPlayer(Transform target)
		{
			float step = fartForce * Time.deltaTime;

			while (Vector3.Distance(transform.position, target.position) > 0.01f)
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, step);
				yield return null;
			}

			transform.position = target.position;
			doneFarting = true;
			rb.isKinematic = true;
			DisableVisuals();
			onSwitchToEndCam();
			onDoneFarting();
			fartBeam.Stop();
		}

		private void DisableVisuals()
		{
			SkinnedMeshRenderer[] meshes = playerVis.GetComponentsInChildren<SkinnedMeshRenderer>();

			foreach (var mesh in meshes)
			{
				mesh.enabled = false;
			}

			SpriteRenderer[] sprites = playerVis.GetComponentsInChildren<SpriteRenderer>();

			foreach (var sprite in sprites)
			{
				sprite.enabled = false;
			}
		}

		// private void AddGravityIfFalling()
		// {
		// 	if (rb.velocity.y < 0)
		// 	{
		// 		rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		// 	}
		// 	else if (rb.velocity.y > 0)
		// 	{
		// 		rb.velocity += Vector3.up * Physics.gravity.y * (lowLaunchMultiplier - 1) * Time.deltaTime;
		// 	}
		// }
	}
}

