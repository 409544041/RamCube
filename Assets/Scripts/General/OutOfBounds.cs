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
				var player = other.GetComponent<PlayerCubeMover>(); //TO DO: replace when player refs
				player.isOutOfBounds = true;
				player.isBoosting = false;
				if (gcRef.glRef.movCubeHandler.movingMoveables == 0)
					player.input = true; //This here else wont let you rewind
				gcRef.rewindPulser.InitiatePulse(InterfaceIDs.Rewind);				
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
