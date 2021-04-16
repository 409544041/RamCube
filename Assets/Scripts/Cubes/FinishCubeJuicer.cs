using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Cubes
{
	public class FinishCubeJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip succesClip = null, failClip = null;
		[SerializeField] ParticleSystem[] impactParticles;
		[SerializeField] ParticleSystem chargingBeamsParticles;
		[SerializeField] float glowRampSpeed = .03f;

		//Cache
		public AudioSource source { get; private set; }
		Animator animator;
		MeshRenderer[] meshes;
		PlayerFartLauncher farter;

		//Actions, events, delegates etc
		public UnityEvent onFinishEvent = new UnityEvent();

		private void Awake() 
		{
			source = GetComponentInChildren<AudioSource>();
			animator = GetComponent<Animator>();
			meshes = GetComponentsInChildren<MeshRenderer>();
			farter = FindObjectOfType<PlayerFartLauncher>();
		}

		private void OnEnable()
		{
			if (farter != null) farter.onDoneFarting += StartBreakingAnimation;
		}

		public void PlaySuccesSound()
		{
			source.clip = succesClip;
			onFinishEvent.Invoke();
		}

		public void PlayFailSound()
		{
			source.clip = failClip;
			onFinishEvent.Invoke();
		}

		private void StartBreakingAnimation() 
		{
			animator.SetTrigger("Open");
		}

		private void StartLightBeams() //Called from animation event
		{
			chargingBeamsParticles.Play();
		}

		private void Impact() //Called from animation event
		{
			foreach (ParticleSystem particle in impactParticles)
			{
				particle.Play();
			}

			foreach (MeshRenderer mesh in meshes)
			{
				mesh.enabled = false;
			}
		}

		private IEnumerator EnableGlowMesh() //Called from animation event
		{
			meshes[1].enabled = true;

			while (meshes[1].materials[3].GetFloat("Glow_Alpha") < 1)
			{
				foreach (Material mat in meshes[1].materials)
				{
					float current = mat.GetFloat("Glow_Alpha");
					mat.SetFloat("Glow_Alpha", current + glowRampSpeed);
				}
				yield return null;
			}

			meshes[0].enabled = false;
		}

		private void OnDisable()
		{
			if (farter != null) farter.onDoneFarting -= StartBreakingAnimation;
		}
	}
}
