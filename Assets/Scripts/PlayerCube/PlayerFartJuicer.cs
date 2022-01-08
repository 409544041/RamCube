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
			beamFartJuice;
		public ParticleSystem fartCharge, fartBeam,
			fartBeamImpact, bulletFartImpact;
		[SerializeField] AudioSource dopplerSource;

		//Cache
		public MMFeedbackWiggle preFartMMWiggle { get; set; }

		private void Awake()
		{
			preFartMMWiggle = preFartJuice.GetComponent<MMFeedbackWiggle>();
		}

		public void PreFartJuice()
		{
			preFartJuice.Initialization();
			preFartJuice.PlayFeedbacks();
		}

		public void BulletFartJuice()
		{
			bulletFartJuice.PlayFeedbacks();
		}

		public void BeamFartJuice()
		{
			beamFartJuice.Initialization();
			beamFartJuice.PlayFeedbacks();
		}

		public void StopBeamFartJuice()
		{
			beamFartJuice.StopFeedbacks();
		}

		public void ShapieRescueFartJuice()
		{
			bulletFartImpact.Play();
			var particle = bulletFartJuice.GetComponent<MMFeedbackParticles>();
			particle.enabled = false;
			bulletFartJuice.PlayFeedbacks();
		}
	}
}
