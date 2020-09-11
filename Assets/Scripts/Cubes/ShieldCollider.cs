using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class ShieldCollider : MonoBehaviour
	{
		//Cache
		PlayerCubeMover cubeMover;
		FeedForwardCube ffCube;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "Environment")
			{
				cubeMover = transform.parent.GetComponent<PlayerCubeMover>();
				ffCube = transform.parent.GetComponent<FeedForwardCube>();

				if (cubeMover) cubeMover.isBoosting = false;

				if (ffCube) ffCube.isBoosting = false;

				Destroy(gameObject);
			}
		}
	}
}
