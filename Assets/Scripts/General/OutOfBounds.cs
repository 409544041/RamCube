using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
using UnityEngine;

namespace Qbism.General
{
	public class OutOfBounds : MonoBehaviour
	{
		//Actions, events, delegates etc
		public event Action<InterfaceIDs> onRewindPulse;

		private void OnTriggerEnter(Collider other) 
		{
			if(other.tag == "Player")
			{
				var player = other.GetComponent<PlayerCubeMover>();
				player.isOutOfBounds = true;
				if (FindObjectOfType<MoveableCubeHandler>().movingMoveables == 0)
					player.input = true; //This here else wont let you rewind
				onRewindPulse(InterfaceIDs.Rewind);				
			}
			
			if(other.tag == "Moveable")
			{
				var moveable = other.GetComponent<MoveableCube>();
				moveable.isBoosting = false;
				moveable.isOutOfBounds = true;
				moveable.mesh.enabled = false;
			}

		}
	}
}
