using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserCube : MonoBehaviour
	{
		//Config parameters
		public float distance = 1;
		[SerializeField] Transform laserOrigin;
		[SerializeField] float radius = .3f;
		[SerializeField] LayerMask chosenLayers;
		[SerializeField] float idleLaserDelay = .5f;
		[SerializeField] LaserRefHolder refs;
		
		//Cache
		PlayerCubeMover mover;
		LaserJuicer juicer;
		CubeHandler cubeHandler;
		FinishCube finish;
		PlayerFartLauncher fartLauncher;


		//States
		public bool shouldTrigger { get; set; } = true;
		float currentLength = 0f;
		bool isClosed = false;
		public bool laserPause { get; set; } = false;
		public float dist { get; set; }
		bool eyeClosedForFinish = false;
		public List<Vector2Int> posInLaserPath { get; set; } = new List<Vector2Int>();

		private void Awake()
		{
			mover = refs.gcRef.pRef.playerMover;
			juicer = refs.juicer;
			cubeHandler = refs.gcRef.glRef.cubeHandler;
			finish = refs.gcRef.finishRef.finishCube;
			fartLauncher = refs.gcRef.pRef.fartLauncher;
		}

		private void OnEnable() 
		{
			if (mover != null) mover.onSetLaserTriggers += SetLaserTrigger;

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
		}

		public void HandleLaser()
		{
			int lengthRoundedDown = (int)(Math.Floor(currentLength));
			Vector3 laserDir = transform.forward;
			bool playerHit = false;

			for (int i = 1; i <= lengthRoundedDown; i++)
			{
				if (playerHit) return;

				Vector3 checkPos = transform.position + (laserDir * i);

				Vector2Int roundedCheckPos = new Vector2Int
					(Mathf.RoundToInt(checkPos.x), Mathf.RoundToInt(checkPos.z));

				if (roundedCheckPos == refs.gcRef.pRef.cubePos.FetchGridPos())
				{
					HandleHittingPlayer(true);
					playerHit = true;
				}
			}

			if (!playerHit) GoIdle();
		}

		public void HandleHittingPlayerInBoost(Vector3 crossPoint, bool bulletFart)
		{
			fartLauncher.SetBulletFartToPos(crossPoint);
			HandleHittingPlayer(bulletFart);
		}

		private void HandleHittingPlayer(bool bulletFart)
		{
			if (Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1)
				&& shouldTrigger)
			{
				shouldTrigger = false;

				if (bulletFart)
				{
					fartLauncher.SetBulletFartBackToParent();
					fartLauncher.FireBulletFart();
				}

				CloseEye();
				isClosed = true;
			}

			else if (!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward),
				-1) && shouldTrigger)
			{
				shouldTrigger = false;
				juicer.TriggerDenyJuice(currentLength);
				refs.gcRef.rewindPulser.InitiatePulse();
				refs.gcRef.pRef.stunJuicer.PlayStunVFX();
				mover.isStunned = true;
			}
		}

		public void GoIdle()
		{
			if (isClosed)
			{
				isClosed = false;
				juicer.TriggerIdleJuice();
				CastDottedLines(dist, distance);
			}
			else
			{
				juicer.TriggerIdleJuice();
				CastDottedLines(dist, distance);
			}
		}

		public void CloseEye()
		{
			juicer.TriggerPassJuice();
			CheckForCubes(transform.forward, 1, (int)(Math.Floor(distance)), false);
		}

		private RaycastHit[] SortedSphereCasts()
		{
			RaycastHit[] hits = Physics.SphereCastAll(laserOrigin.position, radius,
				transform.forward, distance, chosenLayers, QueryTriggerInteraction.Ignore);

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
			bool playerFirstHit = false;
			if (hits.Length <= 0) dist = distance + radius + .2f;
			else
			{
				if (hits[0].collider.tag == "Player") playerFirstHit = true;
				dist = hits[0].distance + radius + .2f;
			}

			if (dist != currentLength && !isClosed)
			{
				var visualDist = dist;
				if (playerFirstHit) dist += 1;
				CastDottedLines(dist, distance);
				juicer.AdjustBeamVisualLength(visualDist);
				currentLength = dist;
			}
		}

		public void CastDottedLines(float dist, float startDist)
		{
			int distRoundDown = (int)(Math.Floor(dist));
			int startDistRoundDown = (int)(Math.Floor(startDist));
			Vector3 laserDir = transform.forward;
			//TO DO: Use this system to get the coordinates
			// to take into acount for boost overlap

			//enables dotted lines on cubes within distance
			CheckForCubes(laserDir, 1, distRoundDown, true);

			//disables dotted lines between actual distance and start distance
			if (startDistRoundDown - distRoundDown > 0)
				CheckForCubes(laserDir, distRoundDown + 1, startDistRoundDown, false);
		}

		private void CheckForCubes(Vector3 laserDir, int iStart, 
			int iCondition, bool enable)
		{
			if (enable) posInLaserPath.Clear();

			//checks each int within the laser distance
			for (int i = iStart; i <= iCondition; i++)
			{
				Vector3 checkPos = transform.position + (laserDir * i);

				Vector2Int roundedCheckPos = new Vector2Int
					(Mathf.RoundToInt(checkPos.x), Mathf.RoundToInt(checkPos.z));

				if (enable) posInLaserPath.Add(roundedCheckPos);

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
