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
		[SerializeField] AudioClip landingSFX, introPop;

		//States
		ParticleSystem speedParticles, landParticles;
		AudioSource source;

		private void Awake() 
		{
			speedParticles = introJuice.
				GetComponent<MMFeedbackParticles>().BoundParticleSystem;
			landParticles = introLandingJuice.
				GetComponent<MMFeedbackParticles>().BoundParticleSystem;
			source = introLandingJuice.
				GetComponent<MMFeedbackAudioSource>().TargetAudioSource;
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
			source.clip = landingSFX;
			introLandingJuice.Initialization();
			introLandingJuice.PlayFeedbacks();
		}

		public void PlayButtPopFX()
		{
			source.clip = introPop;
			source.pitch = .9f;
			source.Play();
			buttPopVFX.Play();
		}

		public void PlayPopVFX()
		{
			source.pitch = 1;
			source.Play();
			popVFX.Play();
		}
	}
}
