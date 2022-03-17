using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
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
				if (gcRef.glRef.movCubeHandler.movingMoveables == 0)
					playerMover.input = true; //This here else wont let you rewind
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
				movRef.movCube.isBoosting = false;
				movRef.movCube.isOutOfBounds = true;
				movRef.mesh.enabled = false;
			}

		}
	}
}
