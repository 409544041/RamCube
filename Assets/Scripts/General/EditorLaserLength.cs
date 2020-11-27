using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.General
{
	[ExecuteInEditMode]
	public class EditorLaserLength : MonoBehaviour
	{
		//Cache
		LaserCube laser;

		private void Awake() 
		{
			laser = GetComponent<LaserCube>();	
		}
		
		private void Start()
		{
			laser.laserBeam.transform.localScale = new Vector3(0.5f, laser.distance, 0.5f);
		}
	}
}
