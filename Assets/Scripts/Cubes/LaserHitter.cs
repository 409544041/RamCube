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
				&& !detector.isClosed)
			{
				if (bulletFart)
				{
					fartLauncher.SetBulletFartBackToParent();
					fartLauncher.FireBulletFart();
				}

				refs.gcRef.pRef.stunJuicer.StopStunVFX();

				if (detector.rewindPulseViaLaser)
				{
					refs.gcRef.rewindPulser.StopPulse();
					detector.rewindPulseViaLaser = false;
				}

				detector.Close();
			}

			else if (!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward),
				-1) && !juicer.isDenying)
			{
				if (detector.isClosed) detector.isClosed = false;
				juicer.TriggerDenyJuice(detector.currentDist);
				detector.rewindPulseViaLaser = true;
				refs.gcRef.rewindPulser.InitiatePulse();
				refs.gcRef.pRef.stunJuicer.PlayStunVFX();
				mover.isStunned = true;
				detector.CastDottedLines(hitDist, detector.distance);
			}
		}
	}
}
