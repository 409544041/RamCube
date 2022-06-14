using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.General
{
	public class OutOfBounds : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameplayCoreRefHolder gcRef;

		private void OnTriggerEnter(Collider other) 
		{
			if (other.tag == "Player")
			{
				var playerMover = gcRef.pRef.playerMover;
				playerMover.isOutOfBounds = true;
				playerMover.isBoosting = false;
				playerMover.isMoving = false;
				if (gcRef.glRef.movCubeHandler.movingMoveables == 0)
				{
					playerMover.allowRewind = true;
					playerMover.allowMoveInput = false;
				}
				gcRef.rewindPulser.InitiatePulse();				
			}

			if (other.tag == "FFCube")
			{
				var ffCube = other.GetComponent<FeedForwardCube>();
				ffCube.isOutOfBounds = true;
			}
			
			if (other.tag == "Moveable")
			{
				var movRef = other.GetComponent<CubeRefHolder>();
				var movCube = movRef.movCube;
				movCube.isBoosting = false;
				movCube.isOutOfBounds = true;
				movRef.mesh.enabled = false;
				movRef.mesh.transform.localScale = Vector3.one;
			}

		}
	}
}
