using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.Cubes
{
	[ExecuteInEditMode]
	public class EditorLaserLength : MonoBehaviour
	{
		//TO DO: We probably don't need this script anymore. Toss it?
		
		//Cache
		LaserCube laser;

		private void Awake() 
		{
			laser = GetComponent<LaserCube>();	
		}
		
		private void Start()
		{
			var main = laser.laserBeam.main;
			main.startSizeZMultiplier = laser.distance;
		}
	}
}
