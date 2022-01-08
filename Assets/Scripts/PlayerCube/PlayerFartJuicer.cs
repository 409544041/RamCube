using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerFartJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks preFartJuice, bulletFartJuice,
			beamFartJuice, sputterJuice;
		public ParticleSystem fartCharge, fartBeam,
			fartBeamImpact, bulletFartImpact, sputterFarts;

		//Cache
		public MMFeedbackWiggle preFartMMWiggle { get; set; }
		Animator animator;

		//States
		float[] sputterFartTimes;
		float timer = 0;
		int sputterIndex = 0;
		bool sputterFarting = false;

		private void Awake()
		{
			preFartMMWiggle = preFartJuice.GetComponent<MMFeedbackWiggle>();
			animator = GetComponentInChildren<Animator>();
		}

		private void Update()
		{
			if (sputterFarting) SputterFartJuice();
		}

		public void PreFartJuice()
		{
			preFartJuice.Initialization();
			preFartJuice.PlayFeedbacks();
		}

		public void BulletFartJuice()
		{
			bulletFartJuice.PlayFeedbacks();
			animator.SetTrigger("FartToot");
		}

		public void BeamFartJuice()
		{
			beamFartJuice.Initialization();
			beamFartJuice.PlayFeedbacks();
			animator.SetBool("FartLoop", true);
		}

		public void StopBeamFartJuice()
		{
			beamFartJuice.StopFeedbacks();
			animator.SetBool("FartLoop", false);
		}

		public void ShapieRescueFartJuice()
		{
			bulletFartImpact.Play();
			animator.SetTrigger("FartToot");
			var particle = bulletFartJuice.GetComponent<MMFeedbackParticles>();
			particle.enabled = false;
			bulletFartJuice.PlayFeedbacks();
		}

		public void TriggerSputterFarts()
		{
			sputterJuice.Initialization();
			var emis = sputterFarts.emission;
			sputterFartTimes = new float[emis.burstCount];

			for (int i = 0; i < emis.burstCount; i++)
			{
				sputterFartTimes[i] = emis.GetBurst(i).time;
			}

			sputterFarting = true;
			sputterFarts.Play();
		}

		private void SputterFartJuice()
		{
			timer += Time.deltaTime;
			if (timer > sputterFartTimes[sputterIndex])
			{
				sputterJuice.PlayFeedbacks();
				animator.SetTrigger("FartToot");
				sputterIndex++;
				if (sputterIndex >= sputterFartTimes.Length) sputterFarting = false;
			}
		}
	}
}
