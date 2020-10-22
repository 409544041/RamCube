using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class BoostCollider : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Environment" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Moveable")
			{
				var cubeMover = transform.parent.GetComponent<PlayerCubeMover>();
				var ffCube = transform.parent.GetComponent<FeedForwardCube>();
				var moveable = transform.parent.GetComponent<MoveableCube>();
				
				if (cubeMover) cubeMover.isBoosting = false;
				if (ffCube) ffCube.isBoosting = false;
				if(moveable) moveable.isBoosting = false;

				Destroy(gameObject);
			}
		}
	}
}
