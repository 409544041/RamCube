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
		public Color neutralColor, passColor, denyColor;
		[SerializeField] GameObject flame;
		public Material neutralFlame, passFlame, denyFlame;

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
			currentColor = neutralColor;
		}

		public void SetLaserColor(Color color, Material mat)
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

			flame.GetComponent<Renderer>().material = mat;

			if(mat == denyFlame)
			{
				flame.transform.localScale = new Vector3(1, 2, 1);
				flame.transform.localPosition = new Vector3(0, 1.75f, 0);
			}
			else
			{
				flame.transform.localScale = new Vector3(1, 1, 1);
				flame.transform.localPosition = new Vector3(0, 1, 0);
			} 

			currentColor = color;
		}

		public void MoveTipLight(float distance)
		{
			laserTipLight.transform.localPosition = new Vector3(0, 0, distance + 0.4f);
		}
	}
}
