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
		[SerializeField] ParticleSystem particles;
		[SerializeField] float torqueForce = 1000;

		public void HandleExplosion(Transform explOrigin)
		{
			SwapMeshes();
			particles.Stop();
			rb.isKinematic = false; 
			rb.AddTorque(explOrigin.right * torqueForce);
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
