using Qbism.Cubes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCollHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] ExplosionForce explosion;

		private void OnCollisionEnter(Collision collision)
		{
			var explHandler = collision.transform.GetComponent<IExplosionHandler>();
			if (explHandler != null) explosion.KnockBack();
		}
	}

}