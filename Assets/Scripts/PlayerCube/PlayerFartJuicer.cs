using System;
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
			beamFartJuice, sputterJuice;
		public ParticleSystem fartCharge, fartBeam,
			fartBeamImpact, bulletFartImpact, sputterFarts;
		[SerializeField] float maxSputterTime = 2;

		//Cache
		public MMFeedbackWiggle preFartMMWiggle { get; set; }
		Animator animator;

		//States
		float[] sputterFartTimes;
		float timer = 0;
		int sputterIndex = 0;
		bool sputterFarting = false;

		private void Awake()
		{
			preFartMMWiggle = preFartJuice.GetComponent<MMFeedbackWiggle>();
			animator = GetComponentInChildren<Animator>();
		}

		private void Update()
		{
			if (sputterFarting) SputterFartJuice();
		}

		public void PreFartJuice()
		{
			preFartJuice.Initialization();
			preFartJuice.PlayFeedbacks();
		}

		public void BulletFartJuice()
		{
			bulletFartJuice.PlayFeedbacks();
			animator.SetTrigger("FartToot");
		}

		public void BeamFartJuice()
		{
			beamFartJuice.Initialization();
			beamFartJuice.PlayFeedbacks();
			animator.SetBool("FartLoop", true);
		}

		public void StopBeamFartJuice()
		{
			beamFartJuice.StopFeedbacks();
			animator.SetBool("FartLoop", false);
		}

		public void ShapieRescueFartJuice()
		{
			bulletFartImpact.Play();
			animator.SetTrigger("FartToot");
			var particle = bulletFartJuice.GetComponent<MMFeedbackParticles>();
			particle.enabled = false;
			bulletFartJuice.PlayFeedbacks();
		}

		public void TriggerSputterFarts()
		{
			RandomizeSputterFarts(); 

			sputterFarting = true;
			sputterJuice.Initialization();
			sputterFarts.Play();
		}

		private void SputterFartJuice()
		{
			timer += Time.deltaTime;
			if (timer > sputterFartTimes[sputterIndex])
			{
				sputterJuice.PlayFeedbacks();
				animator.SetTrigger("FartToot");
				sputterIndex++;
				if (sputterIndex >= sputterFartTimes.Length) sputterFarting = false;
			}
		}

		private void RandomizeSputterFarts()
		{
			var allPartSystems = sputterFarts.GetComponentsInChildren<ParticleSystem>();
			ParticleSystem.EmissionModule[] emisArray = new ParticleSystem.EmissionModule[allPartSystems.Length];
		
			for (int i = 0; i < allPartSystems.Length; i++)
			{
				emisArray[i] = allPartSystems[i].emission;
			}

			//Set possible fart times with .1f gaps
			List<float> possibleTimes = new List<float>();
			float counter = 0;

			for (int i = 0; i < 21; i++)
			{
				possibleTimes.Add(counter);
				counter += .1f;
				if (counter > maxSputterTime) break;
			}

			int tootAmount = UnityEngine.Random.Range(3, 8);

			for (int i = 0; i < emisArray.Length; i++)
			{
				emisArray[i].burstCount = tootAmount;
			}

			sputterFartTimes = new float[tootAmount];
			ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[tootAmount];
			
			//Set toot times
			for (int i = 0; i < tootAmount; i++)
			{
				var j = UnityEngine.Random.Range(0, possibleTimes.Count - 1);
				var tootTime = possibleTimes[j];
				sputterFartTimes[i] = tootTime;
				possibleTimes.RemoveAt(j);

				bursts[i].time = tootTime;
				bursts[i].count = 1;
			}

			Array.Sort(sputterFartTimes, bursts);

			for (int k = 0; k < emisArray.Length; k++)
			{
				emisArray[k].SetBursts(bursts);
			}
		}
	}
}
