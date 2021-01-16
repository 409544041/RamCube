using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Qbism.General
{
	public class CamCentralizer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CinemachineVirtualCamera cam = null;
		[SerializeField] Vector3 camValues = new Vector3 (0, 0, 0);

		void Start()
		{	
			PositionCenterpoint posCenter = GetComponent<PositionCenterpoint>();
			if(!posCenter) PositionCam();
		}

		public void PositionCam()
		{
			cam.transform.position = transform.position + camValues;
		}
	}
}

