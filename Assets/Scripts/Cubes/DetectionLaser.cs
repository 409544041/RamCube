using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class DetectionLaser : MonoBehaviour
	{
		//Config parameters
		public float distance = 1;
		[SerializeField] Transform laserOrigin;
		[SerializeField] float radius = .3f;
		[SerializeField] LayerMask chosenLayers;
		public TotemTypes type;
		[SerializeField] LaserRefHolder refs;
		
		//Cache
		PlayerCubeMover mover;
		LaserJuicer juicer;
		CubeHandler cubeHandler;
		FinishCube finish;
		public ILaserEffector effector { get; private set; }

		//States
		public bool isClosed { get; set; } = false;
		public float currentDist { get; set; } = 0f;
		bool eyeClosedForFinish = false;
		public List<Vector2Int> posInLaserPath { get; set; } = new List<Vector2Int>();
		public bool rewindPulseViaLaser { get; set; } = false;

		private void Awake()
		{
			mover = refs.gcRef.pRef.playerMover;
			juicer = refs.juicer;
			cubeHandler = refs.gcRef.glRef.cubeHandler;
			finish = refs.gcRef.finishRef.finishCube;
			effector = GetComponent<ILaserEffector>();
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
				if (playerHit && !mover.isResetting) 
					effector.HandleHittingPlayer(true, dist);
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

		public void Close()
		{
			isClosed = true;
			juicer.TriggerPassJuice();
			CheckForCubes(transform.forward, 1,
				(int)(Math.Floor(distance)), false);
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

		public void CheckForCubes(Vector3 laserDir, int iStart, 
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
					cube.CastDottedLines(transform.position, enable, type);
				}
			}
		}
	}
}
