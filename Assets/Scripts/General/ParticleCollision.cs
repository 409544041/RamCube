using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.General
{
	public class ParticleCollision : MonoBehaviour
	{
		//Actions, events, delegates etc
		public UnityEvent onParticleCollision = new UnityEvent();

		private void OnParticleCollision(GameObject other) 
		{
			onParticleCollision.Invoke();

			var laser = other.GetComponentInParent<LaserCube>();
			if (laser) laser.CloseEye();
		}
	}
}
