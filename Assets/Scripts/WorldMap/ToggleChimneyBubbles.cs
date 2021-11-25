using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class ToggleChimneyBubbles : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float tresholdY;
		[SerializeField] ParticleSystem particle;

		//States
		bool isPlaying = false;

		private void Update() 
		{
			if (transform.position.y >= tresholdY && !isPlaying)
			{
				particle.Play();
				isPlaying = true;
			} 
		}
	}
}
