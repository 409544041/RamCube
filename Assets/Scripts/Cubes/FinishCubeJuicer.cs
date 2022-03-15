using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Pathfinding;
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
		[Header("Charging VFX")]
		[SerializeField] MMFeedbacks chargeJuice;
		[SerializeField] float glowIncrease = .03f, glowIncreaseInterval = .05f;
		[Header("References")]
		[SerializeField] FinishRefHolder refs;

		//Cache
		PlayerFartLauncher farter;
		PlayerCubeMover player;

		private void Awake() 
		{
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
			if(!refs.finishCube.FetchFinishStatus()) CheckForGlowDeactivation();
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
			refs.source.PlayOneShot(succesClip);
		}

		public void PlayFailSound()
		{
			refs.source.PlayOneShot(failClip);
		}

		private void StartBreakingAnimation() 
		{
			refs.animator.SetTrigger("Open");
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

			refs.nmCutter.enabled = false;
			refs.mesh.enabled = false;
			refs.glowMesh.enabled = false;
			refs.explForce.KnockBack();

			refs.endSeq.Spawn();
		}

		private void StartGlowing()
		{
			StartCoroutine(EnableGlowMesh());
		}

		private IEnumerator EnableGlowMesh() 
		{
			refs.glowMesh.enabled = true;

			while (refs.glowMesh.materials[3].GetFloat("Glow_Alpha") < 1)
			{
				foreach (Material mat in refs.glowMesh.materials)
				{
					float current = mat.GetFloat("Glow_Alpha");
					mat.SetFloat("Glow_Alpha", current + glowIncrease);
				}
				yield return new WaitForSeconds(glowIncreaseInterval);
			}

			refs.mesh.enabled = false;
		}

		private Vector3 FetchFinishPos()
		{
			return refs.glowMesh.transform.position;
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
