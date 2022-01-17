using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class VegetationExplosionHandler : MonoBehaviour, IExplosionHandler
	{
		[SerializeField] ParticleSystem[] vegExplosionVFX;
		public void HandleExplosion(Vector3 explOriginPos)
		{
			foreach (var vfx in vegExplosionVFX)
			{
				var dir = transform.position - explOriginPos;
				vfx.transform.forward = dir;
				vfx.Play();
			}
		}
	}
}
