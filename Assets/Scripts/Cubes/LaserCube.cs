using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Cubes
{
	public class LaserCube : MonoBehaviour
	{
		//Config parameters
		public float distance = 1;
		public GameObject laserBeam = null;
		[SerializeField] Transform laserOrigin = null;
		[SerializeField] LayerMask chosenLayers;

		[Header("Juice Options")]
		[SerializeField] Light laserTipLight;
		[SerializeField] AudioClip passClip = null, denyClip = null;
		[SerializeField] Color neutralColor, passColor, denyColor;		

		//Cache
		PlayerCubeMover mover;
		AudioSource source;
		SceneHandler loader;

		//States
		bool shouldTrigger = true;
		Color currentColor;
		Material beamMat;
		ParticleSystem[] laserParticles;
		Light[] laserLights;

		public UnityEvent onLaserPassEvent = new UnityEvent();

		private void Awake()
		{
			mover = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCubeMover>();
			loader = FindObjectOfType<SceneHandler>();
			source = GetComponentInChildren<AudioSource>();
		}

		private void OnEnable() 
		{
			if(mover != null) mover.onSetLaserTriggers += SetLaserTrigger;
		}

		private void Start()
		{
			mover.lasersInLevel = true;
			beamMat = laserBeam.GetComponent<Renderer>().material;
			laserParticles = GetComponentsInChildren<ParticleSystem>();
			laserLights = GetComponentsInChildren<Light>();
			currentColor = neutralColor;
		}

		private void FixedUpdate()
		{
			FireLaserCast();
		}

		private void FireLaserCast()
		{
			RaycastHit[] hits = SortedRaycasts();

			AdjustBeamLength(hits);

			if (hits.Length > 0 && (mover.input || mover.isBoosting))
			{
				if (hits[0].transform.gameObject.tag == "Player" &&
				Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1))
				{
					if(shouldTrigger)
					{
						SetLaserColor(passColor);
						source.clip = passClip;
						onLaserPassEvent.Invoke();
						shouldTrigger = false;
					}
				}

				else if (hits[0].transform.gameObject.tag == "Player" &&
					!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1))
				{
					if (shouldTrigger)
					{
						SetLaserColor(denyColor);
						StartCoroutine(RestartLevelTransition());
						shouldTrigger = false;
					}
				}
			} 	
		}

		private void AdjustBeamLength(RaycastHit[] hits)
		{
			if (hits.Length > 0)
			{
				laserBeam.transform.localScale = new Vector3(0.5f, hits[0].distance, 0.5f);
				laserBeam.transform.localPosition = new Vector3(0, -0.5f, (.5f * hits[0].distance) + 0.5f);
				laserTipLight.transform.localPosition = new Vector3(0, 0, hits[0].distance + 0.4f);
			}
			else
			{
				SetLaserColor(neutralColor);
				laserBeam.transform.localScale = new Vector3(0.5f, distance, 0.5f);
				laserBeam.transform.localPosition = new Vector3(0, -0.5f, (.5f * distance) + 0.5f);
				laserTipLight.transform.localPosition = new Vector3(0, 0, distance + 0.4f);
			}
		}

		private RaycastHit[] SortedRaycasts()
		{
			RaycastHit[] hits = Physics.RaycastAll(laserOrigin.position,
				transform.TransformDirection(Vector3.forward), distance, chosenLayers , QueryTriggerInteraction.Ignore);

			Debug.DrawRay(laserOrigin.position,
				transform.TransformDirection(Vector3.forward * distance), Color.red, distance);

			float[] hitDistances = new float[hits.Length];

			for (int hit = 0; hit < hitDistances.Length; hit++)
			{
				hitDistances[hit] = hits[hit].distance;
			}

			Array.Sort(hitDistances, hits);

			return hits;
		}

		private void SetLaserColor(Color color)
		{
			if(currentColor == color) return;

			beamMat.color = color;
			
			foreach(ParticleSystem particle in laserParticles)
			{
				var mainModule = particle.main;
				mainModule.startColor = color;
			}
			
			foreach(Light light in laserLights)
			{
				light.color = color;
			}

			currentColor = color;
		}

		private IEnumerator RestartLevelTransition()
		{
			shouldTrigger = false;
			mover.input = false;
			source.clip = denyClip;
			onLaserPassEvent.Invoke();
			yield return new WaitWhile(() => source.isPlaying);
			loader.RestartLevel();
		}

		private void SetLaserTrigger(bool value)
		{
			shouldTrigger = value;
		}

		private void OnDisable()
		{
			if (mover != null) mover.onSetLaserTriggers -= SetLaserTrigger;
		}
	}
}
