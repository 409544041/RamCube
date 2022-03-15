using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserCube : MonoBehaviour
	{
		//Config parameters
		public float distance = 1;
		[SerializeField] Transform laserOrigin = null;
		[SerializeField] LayerMask chosenLayers;
		[SerializeField] float idleLaserDelay = .5f;
		
		//Cache
		PlayerCubeMover mover;
		LaserJuicer juicer;
		CubeHandler cubeHandler;
		FinishCube finish;


		//States
		public bool shouldTrigger { get; set; } = true;
		float currentLength = 0f;
		bool isClosed = false;
		public bool laserPause { get; set; }= false;
		public float dist { get; set; }
		bool eyeClosedForFinish = false;

		//Actions, events, delegates etc
		public event Action<InterfaceIDs> onRewindPulse;

		private void Awake()
		{
			//TO DO: Link to refs once we have laser ref
			mover = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCubeMover>();
			juicer = GetComponent<LaserJuicer>();
			cubeHandler = FindObjectOfType<CubeHandler>();
			finish = FindObjectOfType<FinishCube>(); 
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
			if (!finish.hasFinished) FireSphereCast();
			else if (finish.hasFinished && !eyeClosedForFinish)
			{
				juicer.CloseEyeForFinish();
				eyeClosedForFinish = true;
			}
		}

		private void FireSphereCast()
		{
			RaycastHit[] hits = SortedSphereCasts();

			AdjustBeamLength(hits);

			if (hits.Length > 0 && (mover.input || mover.isBoosting))
			{
				if (hits[0].transform.gameObject.tag == "Player" &&
				Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1))
				{
					if(shouldTrigger)
					{
						shouldTrigger = false;
						var fartLauncher = mover.GetComponent<PlayerFartLauncher>();
						bool hasHit = false;
						RaycastHit hit = fartLauncher.FireRayCast(out hasHit);
						if (hasHit) fartLauncher.FireBulletFart();
						isClosed = true;
					}
				}

				else if (hits[0].transform.gameObject.tag == "Player" &&
					!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1))
				{
					if (shouldTrigger)
					{
						shouldTrigger = false;
						juicer.TriggerDenyJuice(currentLength);
						onRewindPulse(InterfaceIDs.Rewind);
						mover.GetComponent<PlayerStunJuicer>().PlayStunVFX();
						mover.isStunned = true;						
					}
				}

				else GoIdle();
			}

			else GoIdle();
		}

		private void GoIdle()
		{
			if (isClosed && !laserPause)
			{
				isClosed = false;
				laserPause = true;
				StartCoroutine(TriggerIdleLaserOnDelay());
			}
			else if (!isClosed && !laserPause)
			{
				juicer.TriggerIdleJuice();
			}
		}

		private IEnumerator TriggerIdleLaserOnDelay()
		{
			//So laser turns back on when player has had time to move on
			yield return new WaitForSeconds(idleLaserDelay);
			juicer.TriggerIdleJuice();
			laserPause = false;
		}

		public void CloseEye() //Called from fart particle collision
		{
			juicer.TriggerPassJuice();
			CheckForCubes(transform.forward, 1, (int)(Math.Floor(distance)), false);
		}

		private RaycastHit[] SortedSphereCasts()
		{
			RaycastHit[] hits = Physics.SphereCastAll(laserOrigin.position, .05f,
				transform.forward, distance, chosenLayers , QueryTriggerInteraction.Ignore);
			
			Debug.DrawRay(laserOrigin.position, transform.forward, Color.red, distance);

			float[] hitDistances = new float[hits.Length];

			for (int hit = 0; hit < hitDistances.Length; hit++)
			{
				hitDistances[hit] = hits[hit].distance;
			}

			Array.Sort(hitDistances, hits);

			return hits;
		}

		private void AdjustBeamLength(RaycastHit[] hits)
		{
			if (hits.Length <= 0) dist = distance;
			else dist = hits[0].distance;

			if (dist != currentLength)
			{
				CastDottedLines(dist, distance);
				juicer.AdjustBeamVisualLength(dist);
				currentLength = dist;
			}
		}

		public void CastDottedLines(float dist, float startDist)
		{
			int distRoundDown = (int)(Math.Floor(dist));
			int startDistRoundDown = (int)(Math.Floor(startDist));
			Vector3 laserDir = transform.forward;

			//enables dotted lines on cubes within distance
			CheckForCubes(laserDir, 1, distRoundDown, true);

			//disables dotted lines between actual distance and start distance
			if (startDistRoundDown - distRoundDown > 0)
				CheckForCubes(laserDir, distRoundDown + 1, startDistRoundDown, false);
		}

		private void CheckForCubes(Vector3 laserDir, int iStart, 
			int iCondition, bool enable)
		{
			//checks each int within the laser distance
			for (int i = iStart; i <= iCondition; i++)
			{
				Vector3 checkPos = transform.position + (laserDir * i);

				Vector2Int roundedCheckPos = new Vector2Int
					(Mathf.RoundToInt(checkPos.x), Mathf.RoundToInt(checkPos.z));

				//checks if point has a cube
				if (cubeHandler.CheckFloorCubeDicKey(roundedCheckPos))
				{
					var cube = cubeHandler.FetchCube(roundedCheckPos, true);
					cube.CastDottedLines(transform.position, enable);
				}
			}
		}

		public void SetLaserTrigger(bool value)
		{
			shouldTrigger = value;
		}

		private void OnDisable()
		{
			if (mover != null) mover.onSetLaserTriggers -= SetLaserTrigger;
		}
	}
}
