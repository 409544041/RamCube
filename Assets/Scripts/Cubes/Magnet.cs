using Qbism.PlayerCube;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class Magnet : MonoBehaviour, ILaserEffector
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

				refs.gcRef.pRef.stunJuicer.StopStunVFX(TotemTypes.magnet);

				if (detector.rewindPulseViaLaser)
				{
					refs.gcRef.rewindPulser.StopPulse();
					detector.rewindPulseViaLaser = false;
				}

				detector.Close();

				if (mover.isBeingPulled) mover.isBeingPulled = false;
			}

			else if (!Mathf.Approximately(Vector3.Dot(mover.transform.forward, transform.forward), -1))
			{
				if (detector.isClosed) detector.isClosed = false;

				Transform side;
				Vector3 turnAxis;
				Vector2Int posAhead;
				bool isNextToMagnet;
				
				GetPullDirection(out side, out turnAxis, out posAhead, out isNextToMagnet);

				if (isNextToMagnet)
				{
					mover.isBeingPulled = false;
					mover.allowRewind = true;
					detector.GoIdle();
					refs.gcRef.pRef.stunJuicer.StopStunVFX(TotemTypes.magnet);
				}

				if (!isNextToMagnet)
				{
					mover.isBeingPulled = true;
					juicer.TriggerActivationJuice();
					refs.gcRef.pRef.stunJuicer.PlayStunVFX(TotemTypes.magnet);
					mover.allowRewind = false;
					if (!mover.isBoosting) mover.InitiateFromMagnet(side, turnAxis, posAhead);
				} 
				
				detector.CastDottedLines(hitDist, detector.distance);
			}
		}

		private void GetPullDirection(out Transform side, out Vector3 turnAxis,
			out Vector2Int posAhead, out bool isNextToMagnet)
		{
			var pos = FetchGridPos();
			var playerPos = refs.gcRef.pRef.cubePos.FetchGridPos();
			var dir = (pos - playerPos);

			if (dir.x == 1 || dir.x == -1 || dir.y == 1 || dir.y == -1) isNextToMagnet = true;
			else isNextToMagnet = false;

			if (dir.x > 0)
			{
				side = mover.right;
				turnAxis = Vector3.back;
				posAhead = playerPos + Vector2Int.right;
			}

			else if (dir.x < 0)
			{
				side = mover.left;
				turnAxis = Vector3.forward;
				posAhead = playerPos + Vector2Int.left;
			}
			else if (dir.y > 0)
			{
				side = mover.up;
				turnAxis = Vector3.right;
				posAhead = playerPos + Vector2Int.up;
			}
			else
			{
				side = mover.down;
				turnAxis = Vector3.left;
				posAhead = playerPos + Vector2Int.down;
			}
		}

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}
	}
}
