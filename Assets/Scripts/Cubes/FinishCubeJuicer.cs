﻿using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.PlayerCube;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Cubes
{
	public class FinishCubeJuicer : MonoBehaviour
	{
		//Config parameters
		[Header("SFX")]
		[SerializeField] AudioClip succesClip = null;
		[SerializeField] AudioClip failClip = null;
		[Header("VFX")]
		[SerializeField] GameObject glowEmitParticles;
		[Header("Impact VFX")]
		[SerializeField] MMFeedbacks impactJuice;
		[SerializeField] MeshRenderer mesh, glowMesh;
		[SerializeField] NavMeshObstacle navMeshOb;
		[Header("Charging VFX")]
		[SerializeField] MMFeedbacks chargeJuice;
		[SerializeField] float glowIncrease = .03f, glowIncreaseInterval = .05f;
		[Header("References")]
		[SerializeField] ExplosionForce explosion;

		//Cache
		public AudioSource source { get; private set; }
		Animator animator;
		PlayerFartLauncher farter;
		PlayerCubeMover player;

		//Actions, events, delegates etc
		public event Action onSpawnFriends;
		public Func<bool> onFinishCheck;

		private void Awake() 
		{
			source = GetComponentInChildren<AudioSource>();
			animator = GetComponent<Animator>();
			player = FindObjectOfType<PlayerCubeMover>();
			farter = player.GetComponent<PlayerFartLauncher>();
		}

		private void OnEnable()
		{
			if (farter != null)
			{
				farter.onDoneFarting += StartBreakingAnimation;
				farter.onStartFarting += StartGlowing;
				farter.onCheckFinishPos += FetchFinishPos;
			} 
		}

		private void Update()
		{
			if(!onFinishCheck()) CheckForGlowDeactivation();
		}

		private void CheckForGlowDeactivation()
		{
			if (Vector3.Distance(transform.position, player.transform.position) < 1.25f)
				glowEmitParticles.SetActive(false);
			else if (glowEmitParticles.activeSelf == false) glowEmitParticles.SetActive(true);
		}

		public void DeactivateGlow()
		{
			glowEmitParticles.SetActive(false);
		}

		public void PlaySuccesSound()
		{
			source.PlayOneShot(succesClip);
		}

		public void PlayFailSound()
		{
			source.PlayOneShot(failClip);
		}

		private void StartBreakingAnimation() 
		{
			animator.SetTrigger("Open");
		}

		private void StartChargeFX() //Called from animation event
		{
			chargeJuice.PlayFeedbacks();
		}

		private void StopChargeFX() //Called from animation event
		{
			chargeJuice.StopFeedbacks();
		}

		public void Impact() //Called from animation event
		{
			impactJuice.PlayFeedbacks();

			navMeshOb.enabled = false;
			mesh.enabled = false;
			glowMesh.enabled = false;
			explosion.KnockBack();

			onSpawnFriends();
		}

		private void StartGlowing()
		{
			StartCoroutine(EnableGlowMesh());
		}

		private IEnumerator EnableGlowMesh() 
		{
			glowMesh.enabled = true;

			while (glowMesh.materials[3].GetFloat("Glow_Alpha") < 1)
			{
				foreach (Material mat in glowMesh.materials)
				{
					float current = mat.GetFloat("Glow_Alpha");
					mat.SetFloat("Glow_Alpha", current + glowIncrease);
				}
				yield return new WaitForSeconds(glowIncreaseInterval);
			}

			mesh.enabled = false;
		}

		private Vector3 FetchFinishPos()
		{
			return glowMesh.transform.position;
		}

		private void OnDisable()
		{
			if (farter != null)
			{
				farter.onDoneFarting -= StartBreakingAnimation;
				farter.onStartFarting -= StartGlowing;
				farter.onCheckFinishPos -= FetchFinishPos;
			}
		}
	}
}
