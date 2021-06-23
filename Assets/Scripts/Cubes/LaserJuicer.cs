using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Cubes
{
	public class LaserJuicer : MonoBehaviour
	{
		//Config parameters
		public Light laserTipLight;
		public AudioClip passClip = null, denyClip = null;
		public ParticleSystem laserBeam, denyBeam, denySunSpots, pinkEyeVFX;

		//Cache
		AudioSource source;
		LaserMouthAnimator mouthAnim;
		LaserEyeAnimator eyeAnim;

		//Actions, events, delegates etc
		public UnityEvent onLaserPassEvent = new UnityEvent();

		private void Awake() 
		{
			source = GetComponentInChildren<AudioSource>();
			eyeAnim = GetComponentInChildren<LaserEyeAnimator>();
			mouthAnim = GetComponentInChildren<LaserMouthAnimator>();
		}

		public void AdjustBeamVisualLength(float dist)
		{
			var idleMain = laserBeam.main;
			idleMain.startSizeZMultiplier = dist;

			var denyMain = denyBeam.main;
			denyMain.startSizeZMultiplier = dist;

			var shape = denySunSpots.shape;
			shape.length = dist;

			MoveTipLight(dist);
		}

		public void TriggerPassJuice()
		{
			laserBeam.Stop();
			denyBeam.Stop();
			pinkEyeVFX.Play();

			source.clip = passClip;
			onLaserPassEvent.Invoke();

			eyeAnim.CloseEyes();
			mouthAnim.SadMouth();

		}

		public void TriggerDenyJuice(float dist)
		{
			laserBeam.Stop();
			pinkEyeVFX.Stop();			
			denyBeam.Play();

			source.clip = denyClip;
			onLaserPassEvent.Invoke();

			eyeAnim.OpenEyes();
			mouthAnim.HappyMouth();
		}

		public void TriggerIdleJuice()
		{
			pinkEyeVFX.Stop();
			denyBeam.Stop();
			laserBeam.Play();

			eyeAnim.OpenEyes();
			mouthAnim.HappyMouth();
		}

		private void MoveTipLight(float dist)
		{
			laserTipLight.transform.localPosition = new Vector3(0, 0, dist + 0.4f);
		}
	}
}
