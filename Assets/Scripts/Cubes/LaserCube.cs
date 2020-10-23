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
		[SerializeField] float distance = 1;
		[SerializeField] GameObject laserBeam = null;
		[SerializeField] Transform laserOrigin = null;
		[SerializeField] AudioClip passClip = null, denyClip = null;
		[SerializeField] LayerMask chosenLayers;

		//Cache
		PlayerCubeMover mover;
		AudioSource source;
		SceneHandler loader;

		//States
		bool shouldMakeSound = true;

		public UnityEvent onLaserPassEvent = new UnityEvent();

		private void Awake()
		{
			mover = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCubeMover>();
			loader = FindObjectOfType<SceneHandler>();
			source = GetComponentInChildren<AudioSource>();
		}

		private void Start()
		{
			laserBeam.transform.localScale = new Vector3(1, 1, distance);
			laserBeam.transform.localPosition = new Vector3(0, -0.5f, (.5f * distance) + 0.5f);
		}

		void FixedUpdate()
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
					if(shouldMakeSound)
					{
						source.clip = passClip;
						onLaserPassEvent.Invoke();
						shouldMakeSound = false;
					}
				}

				else if (hits[0].transform.gameObject.tag == "Player" &&
					!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1))
				{
					StartCoroutine(RestartLevelTransition());
				}
			} 	
		}

		private void AdjustBeamLength(RaycastHit[] hits)
		{
			if (hits.Length > 0)
			{
				laserBeam.transform.localScale = new Vector3(1, 1, hits[0].distance);
				laserBeam.transform.localPosition = new Vector3(0, -0.5f, (.5f * hits[0].distance) + 0.5f);
			}
			else
			{
				shouldMakeSound = true;
				laserBeam.transform.localScale = new Vector3(1, 1, distance);
				laserBeam.transform.localPosition = new Vector3(0, -0.5f, (.5f * distance) + 0.5f);
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

		private IEnumerator RestartLevelTransition()
		{
			shouldMakeSound = false;
			mover.input = false;
			source.clip = denyClip;
			onLaserPassEvent.Invoke();
			yield return new WaitWhile(() => source.isPlaying);
			loader.RestartLevel();
		}
	}
}
