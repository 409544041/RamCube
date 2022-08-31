using Qbism.PlayerCube;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserHitter : MonoBehaviour, ILaserEffector
	{
		//Config parameters
		[SerializeField] LaserRefHolder refs;

		//Cache
		PlayerFartLauncher fartLauncher;
		PlayerCubeMover mover;
		LaserJuicer juicer;
		DetectionLaser detector;

		//States
		public bool isClosed { get; set; } = false;
		bool rewindPulseViaLaser = false;

		private void Awake()
		{
			fartLauncher = refs.gcRef.pRef.fartLauncher;
			mover = refs.gcRef.pRef.playerMover;
			juicer = refs.juicer;
			detector = refs.detector;
		}

		public void HandleHittingPlayerInBoost(Vector3 crossPoint, bool bulletFart)
		{
			if (refs.gcRef.pRef.playerMover.isStunned) return;
			fartLauncher.SetBulletFartToPos(crossPoint);
			HandleHittingPlayer(bulletFart, detector.distance);
		}

		public void HandleHittingPlayer(bool bulletFart, float hitDist)
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
				Close();
			}

			else if (!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward),
				-1) && !juicer.isDenying)
			{
				if (isClosed) isClosed = false;
				juicer.TriggerDenyJuice(detector.currentDist);
				rewindPulseViaLaser = true;
				refs.gcRef.rewindPulser.InitiatePulse();
				refs.gcRef.pRef.stunJuicer.PlayStunVFX();
				mover.isStunned = true;
				detector.CastDottedLines(hitDist, detector.distance);
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
			detector.CheckForCubes(transform.forward, 1, 
				(int)(Math.Floor(detector.distance)), false);
		}

		public bool GetClosedStatus()
		{
			return isClosed;
		}
	}
}
