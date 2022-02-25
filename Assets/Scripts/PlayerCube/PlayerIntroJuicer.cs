using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Peep;
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
		ParticleSystem speedParticles;

		private void Awake() 
		{
			speedParticles = introJuice.
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

			var peepStateManagers = FindObjectsOfType<PeepStateManager>();
			foreach (var peep in peepStateManagers)
			{
				peep.refs.investigateState.player = this.gameObject;
				peep.SwitchState(peep.refs.investigateState);
			}
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
