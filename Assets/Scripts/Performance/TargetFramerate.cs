using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Performance
{
	public class TargetFramerate : MonoBehaviour
	{
		//Config parameters
		[SerializeField] int targetRate = 60;

		private void Start()
		{
			QualitySettings.vSyncCount = 0;
		}

		private void Update()
		{
			if (Application.targetFrameRate != targetRate)
				Application.targetFrameRate = targetRate;
		}
	}
}
