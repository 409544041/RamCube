using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserCollHandler : MonoBehaviour, IExplosionHandler
	{
		//Config parameters
		[SerializeField] BoxCollider wallCollider;
		[SerializeField] Rigidbody rb;
		[SerializeField] float torqueForce = 10000;

		public void HandleExplosion(Transform explTrans)
		{
			if (wallCollider.enabled == true) wallCollider.enabled = false;
			if (rb.isKinematic == true) rb.isKinematic = false;

			rb.AddTorque((transform.position - explTrans.position) * torqueForce);
		}
	}
}
