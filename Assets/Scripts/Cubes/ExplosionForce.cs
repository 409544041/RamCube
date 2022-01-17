using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class ExplosionForce : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float explForce = 500, explRadius = 5;
		[SerializeField] LayerMask affectedLayers;

		public void KnockBack()
		{
			Collider[] colls = Physics.OverlapSphere(transform.position, explRadius, affectedLayers);

			foreach (var coll in colls)
			{
				var explHandler = coll.GetComponent<IExplosionHandler>();
				if (explHandler == null) continue;

				var rb = coll.GetComponent<Rigidbody>();
				
				explHandler.HandleExplosion(transform.position);

				if (rb != null)
					rb.AddExplosionForce(explForce, transform.position, explRadius);
			}
		}
	}
}
