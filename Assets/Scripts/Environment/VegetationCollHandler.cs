using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class VegetationCollHandler : MonoBehaviour, IExplosionHandler, ISerpentCollHandler
	{
		//Config parameters
		[SerializeField] ParticleSystem[] vegExplosionVFX;
		[SerializeField] MeshRenderer[] meshes;
		[SerializeField] Collider[] colliders;

		public void HandleExplosion(Transform explTrans)
		{
			foreach (var vfx in vegExplosionVFX)
			{
				var dir = transform.position - explTrans.position;
				vfx.transform.forward = dir;
				vfx.Play();

				foreach (var mesh in meshes)
				{
					mesh.enabled = false;
				}

				foreach (var coll in colliders)
				{
					coll.enabled = false;
				}
			}
		}

		public void HandleSerpentColl(Transform serpTrans)
		{
			HandleExplosion(serpTrans);
		}
	}
}
