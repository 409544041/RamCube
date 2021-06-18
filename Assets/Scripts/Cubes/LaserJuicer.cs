using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserJuicer : MonoBehaviour
	{
		//Config parameters
		public Light laserTipLight;
		public AudioClip passClip = null, denyClip = null;
		public Color neutralColor, denyColor;
		public ParticleSystem pinkEyeVFX;

		//States
		Color currentColor;
		Material beamMat;
		ParticleSystem[] laserParticles;
		Light[] laserLights;

		private void Awake() 
		{
			beamMat = GetComponent<LaserCube>().laserBeam.GetComponent<Renderer>().material;
			laserParticles = GetComponentsInChildren<ParticleSystem>();
			laserLights = GetComponentsInChildren<Light>();
		}

		private void Start() 
		{
			beamMat.color = neutralColor;
			currentColor = neutralColor;
		}

		public void SetLaserColor(Color color)
		{
			if (currentColor == color) return;

			beamMat.color = color;

			foreach (ParticleSystem particle in laserParticles)
			{
				var mainModule = particle.main;
				mainModule.startColor = color;
			}

			foreach (Light light in laserLights)
			{
				light.color = color;
			}

			currentColor = color;
		}

		public void MoveTipLight(float distance)
		{
			laserTipLight.transform.localPosition = new Vector3(0, 0, distance + 0.4f);
		}
	}
}
