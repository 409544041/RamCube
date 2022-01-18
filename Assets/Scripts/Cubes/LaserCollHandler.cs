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
		[SerializeField] Rigidbody[] otherRB;
		[SerializeField] float torqueForce = 10000;
		[SerializeField] ParticleSystem impactVFX;

		public void HandleExplosion(Transform explTrans)
		{
			if (wallCollider.enabled == true) wallCollider.enabled = false;
			if (rb.isKinematic == true) rb.isKinematic = false;

			foreach (var rb in otherRB)
			{
				rb.isKinematic = false;
			}

			rb.AddTorque((transform.position - explTrans.position) * torqueForce);
			impactVFX.Play();
		}
	}
}
