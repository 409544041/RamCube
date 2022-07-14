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
		float currentDist = 0f;
		bool isClosed = false;
		public bool laserPause { get; set; } = false;
		bool eyeClosedForFinish = false;
		public List<Vector2Int> posInLaserPath { get; set; } = new List<Vector2Int>();
		bool rewindPulseViaLaser = false;

		private void Awake()
		{
			mover = refs.gcRef.pRef.playerMover;
			juicer = refs.juicer;
			cubeHandler = refs.gcRef.glRef.cubeHandler;
			finish = refs.gcRef.finishRef.finishCube;
			fartLauncher = refs.gcRef.pRef.fartLauncher;
		}

		private void Start()
		{
			HandleLaser();
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
			int lengthRoundedDown = (int)(Math.Floor(distance));
			Vector3 laserDir = transform.forward;
			bool playerHit = false;

			var hitDist = CheckLengthForHit(lengthRoundedDown, out playerHit, laserDir);
			var dist = (int)Math.Floor(hitDist);

			if (hitDist < distance)
			{
				if (playerHit && !mover.isResetting) HandleHittingPlayer(true, dist);
				else
				{
					GoIdle();
					CastDottedLines(dist, distance);
				}
			}
			else
			{
				GoIdle();
				CastDottedLines(distance, distance);
			}
		}

		private float CheckLengthForHit(int lengthRoundedDown, out bool playerHit, Vector3 laserDir)
		{
			for (int i = 1; i <= lengthRoundedDown; i++)
			{
				Vector3 checkPos = transform.position + (laserDir * i);

				Vector2Int roundedCheckPos = new Vector2Int
					(Mathf.RoundToInt(checkPos.x), Mathf.RoundToInt(checkPos.z));

				var distPointToCheck = new Vector3(checkPos.x, laserOrigin.position.y, checkPos.z);
				var dist = Vector3.Distance(distPointToCheck, laserOrigin.position);

				if (roundedCheckPos == refs.gcRef.pRef.cubePos.FetchGridPos())
				{
					playerHit = true;
					return dist;
				}

				foreach (var moveable in refs.gcRef.movCubes)
				{
					if (roundedCheckPos == moveable.refs.cubePos.FetchGridPos() &&
						moveable.refs.floorCube == null)
					{
						playerHit = false;
						return dist;
					}
				}
			}

			playerHit = false;
			return distance;
		}

		public void HandleHittingPlayerInBoost(Vector3 crossPoint, bool bulletFart)
		{
			if (refs.gcRef.pRef.playerMover.isStunned) return;
			fartLauncher.SetBulletFartToPos(crossPoint);
			HandleHittingPlayer(bulletFart, distance);
		}

		private void HandleHittingPlayer(bool bulletFart, float hitDist)
		{
			if (Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1)
				&& isClosed == false)
			{
				if (bulletFart)
				{
					fartLauncher.SetBulletFartBackToParent();
					fartLauncher.FireBulletFart();
				}

				refs.gcRef.pRef.stunJuicer.StopStunVFX();

				if (rewindPulseViaLaser)
				{
					refs.gcRef.rewindPulser.StopPulse();
					rewindPulseViaLaser = false;
				}
				CloseEye();
			}

			else if (!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward),
				-1) && !juicer.isDenying)
			{
				if (isClosed) isClosed = false;
				juicer.TriggerDenyJuice(currentDist);
				rewindPulseViaLaser = true;
				refs.gcRef.rewindPulser.InitiatePulse();
				refs.gcRef.pRef.stunJuicer.PlayStunVFX();
				mover.isStunned = true;
				CastDottedLines(hitDist, distance);
			}
		}

		public void GoIdle()
		{
			if (isClosed) isClosed = false;

			if (rewindPulseViaLaser)
			{
				refs.gcRef.rewindPulser.StopPulse();
				rewindPulseViaLaser = false;
			}

			juicer.TriggerIdleJuice();
		}

		public void CloseEye()
		{
			isClosed = true;
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
			float dist;
			if (hits.Length == 0) dist = distance + radius + .2f;
			else dist = hits[0].distance + radius + .2f;

			if (dist != currentDist && !isClosed)
			{
				juicer.AdjustBeamVisualLength(dist);
				currentDist = dist;
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
	}
}
