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
		public float laserThickness = .25f;
		[SerializeField] LayerMask chosenLayers;
		
		//Cache
		PlayerCubeMover mover;
		AudioSource source;
		SceneHandler loader;
		LaserJuicer juicer;

		//States
		bool shouldTrigger = true;

		public UnityEvent onLaserPassEvent = new UnityEvent();

		private void Awake()
		{
			mover = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCubeMover>();
			loader = FindObjectOfType<SceneHandler>();
			source = GetComponentInChildren<AudioSource>();
			juicer = GetComponent<LaserJuicer>();
		}

		private void OnEnable() 
		{
			if(mover != null) mover.onSetLaserTriggers += SetLaserTrigger;
		}

		private void Start()
		{
			mover.lasersInLevel = true;
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
						juicer.SetLaserColor(juicer.passColor);
						source.clip = juicer.passClip;
						onLaserPassEvent.Invoke();
						shouldTrigger = false;
					}
				}

				else if (hits[0].transform.gameObject.tag == "Player" &&
					!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1))
				{
					if (shouldTrigger)
					{
						juicer.SetLaserColor(juicer.denyColor);
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
				laserBeam.transform.localScale = new Vector3(laserThickness, hits[0].distance, laserThickness);
				juicer.MoveTipLight(hits[0].distance);
			}
			else
			{
				juicer.SetLaserColor(juicer.neutralColor);
				laserBeam.transform.localScale = new Vector3(laserThickness, distance, laserThickness);
				juicer.MoveTipLight(distance);
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
			shouldTrigger = false;
			mover.input = false;
			source.clip = juicer.denyClip;
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
