using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class ChimneyCollHandler : MonoBehaviour, IExplosionHandler, ISerpentCollHandler
	{
		//Config parameters
		[SerializeField] MeshRenderer mesh;
		[SerializeField] MeshRenderer intactMesh;
		[SerializeField] Rigidbody rb;
		[SerializeField] Rigidbody[] otherRB;
		[SerializeField] ParticleSystem bubbleVFX, impactVFX;
		[SerializeField] float torqueForce = 10000;

		public void HandleExplosion(Transform explOrigin)
		{
			SwapMeshes();
			bubbleVFX.Stop();
			impactVFX.Play();
			rb.isKinematic = false; 
			rb.AddTorque(explOrigin.right * torqueForce);
			
			foreach (var rb in otherRB)
			{
				rb.isKinematic = false;
			}
		}

		public void HandleSerpentColl(Transform serpTrans)
		{
			HandleExplosion(serpTrans);
		}

		private void SwapMeshes()
		{
			if (intactMesh.enabled == true) intactMesh.enabled = false;
			mesh.enabled = true;
		}
	}
}
