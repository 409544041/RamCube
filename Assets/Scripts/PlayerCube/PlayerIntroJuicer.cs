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
		[SerializeField] AudioClip introPop;
		[SerializeField] AudioSource audioSource;

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
			audioSource.pitch = .9f;
			audioSource.PlayOneShot(introPop);
			buttPopVFX.Play();
		}

		public void PlayPopVFX()
		{
			audioSource.pitch = 1;
			audioSource.PlayOneShot(introPop);
			popVFX.Play();
		}
	}
}
