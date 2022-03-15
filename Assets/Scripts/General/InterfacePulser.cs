using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
using UnityEngine;

namespace Qbism.General
{
	public class InterfacePulser : MonoBehaviour
	{
		//Config parameters
		[SerializeField] InterfaceIDs interfaceID;
		[SerializeField] MMFeedbacks pulser = null;

		//Cache
		FinishCube finishCube = null;
		LaserCube[] lasers = null;
		TimeBody[] bodies = null;
		PlayerCubeMover playerMover;

		private void Awake()
		{
			//TO DO: Reverse + ref these
			finishCube = FindObjectOfType<FinishCube>(); //TO DO player/finish/cube refs
			lasers = FindObjectsOfType<LaserCube>();
			bodies = FindObjectsOfType<TimeBody>();
			playerMover = FindObjectOfType<PlayerCubeMover>();
		}

		private void OnEnable()
		{
			if (finishCube != null)
			{
				finishCube.onRewindPulse += InitiatePulse;
				finishCube.onStopRewindPulse += StopPulse;
			}

			foreach (LaserCube laser in lasers)
			{
				if (laser != null) laser.onRewindPulse += InitiatePulse;
			}
			
			foreach (TimeBody body in bodies)
			{
				if (body != null) body.onStopRewindPulse += StopPulse;
			}

			if (playerMover != null) playerMover.onRewindPulse += InitiatePulse;
		}

		public void InitiatePulse(InterfaceIDs id) //TO DO: Remove need for interfaceIDs
		{
			if (pulser.IsPlaying) return; 
			
			if (id == interfaceID)
			{
				pulser.Initialization();
				pulser.PlayFeedbacks();
			}
		}

		public void StopPulse(InterfaceIDs id)
		{
			pulser.StopFeedbacks();
		}

		private void OnDisable()
		{
			if (finishCube != null)
			{
				finishCube.onRewindPulse -= InitiatePulse;
				finishCube.onStopRewindPulse -= StopPulse;
			}

			foreach (LaserCube laser in lasers)
			{
				if (laser != null) laser.onRewindPulse -= InitiatePulse;
			}

			foreach (TimeBody body in bodies)
			{
				if (body != null) body.onStopRewindPulse -= StopPulse;
			}

			if (playerMover != null) playerMover.onRewindPulse -= InitiatePulse;
		}
	}

}