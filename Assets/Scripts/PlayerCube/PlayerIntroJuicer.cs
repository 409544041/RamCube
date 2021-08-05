using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerIntroJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks introJuice, introLandingJuice;
		[SerializeField] ParticleSystem buttPopVFX, popVFX;

		//States
		ParticleSystem speedParticles, landParticles;

		private void Awake() 
		{
			speedParticles = introJuice.
				GetComponent<MMFeedbackParticles>().BoundParticleSystem;
			landParticles = introLandingJuice.
				GetComponent<MMFeedbackParticles>().BoundParticleSystem;
		}

		public void TriggerSpeedJuice()
		{
			speedParticles.transform.forward = Vector3.down;
			introJuice.Initialization();
			introJuice.PlayFeedbacks();
		}

		public void TriggerIntroLandingJuice()
		{
			speedParticles.Stop();
			introLandingJuice.Initialization();
			introLandingJuice.PlayFeedbacks();
		}

		public void PlayButtPopFX()
		{
			buttPopVFX.Play();
		}

		public void PlayPopVFX()
		{
			popVFX.Play();
		}
	}
}
