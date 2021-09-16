using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Qbism.General
{
	public class CamCentralizer : MonoBehaviour
	{
		//Config parameters
		[Header("Camera")]
		[SerializeField] CinemachineVirtualCamera cam = null;
		[SerializeField] Vector3 camValues = new Vector3 (0, 0, 0);
		[Header("Debug")]
		[SerializeField] bool CenterAtUpdate = false;

		void Start()
		{	
			PositionCam();
		}

		private void Update() 
		{
			if (CenterAtUpdate) PositionCam();
		}

		public void PositionCam()
		{
			cam.transform.position = transform.position + camValues;
		}
	}
}

