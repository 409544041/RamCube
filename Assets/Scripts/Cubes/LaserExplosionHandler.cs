using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserExplosionHandler : MonoBehaviour, IExplosionHandler
	{
		//Config parameters
		[SerializeField] BoxCollider wallCollider;
		[SerializeField] BoxCollider coll;
		[SerializeField] Rigidbody rb;

		public void HandleExplosion(Vector3 explOriginPos)
		{
			if (wallCollider.enabled == true) wallCollider.enabled = false;
			coll.enabled = true;
			rb.isKinematic = false;
		}
	}
}
