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
		[SerializeField] PlayerRefHolder refs;

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

			List<PeepStateManager> peepStateManagers = new List<PeepStateManager>();

			var peepRefs = refs.gcRef.peeps;
			foreach (var peep in peepRefs)
			{
				peepStateManagers.Add(peep.stateMngr);
			}

			foreach (var peep in peepStateManagers)
			{
				if (peep.peepJob == PeepJobs.balloon) continue;

				peep.refs.stateMngr.player = this.gameObject;
				peep.SwitchState(peep.refs.investigateState);
			}
		}

		public void PlayButtPopFX()
		{
			refs.source.pitch = .9f;
			refs.source.PlayOneShot(introPop);
			buttPopVFX.Play();
		}

		public void PlayPopVFX()
		{
			refs.source.pitch = 1;
			refs.source.PlayOneShot(introPop);
			popVFX.Play();
		}
	}
}
