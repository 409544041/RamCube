using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using UnityEngine;

namespace Qbism.Control
{
	public class MapInputDetector : MonoBehaviour
	{
		//Cache
		GameControls controls;

		void Awake()
		{
			controls = new GameControls();

			controls.Gameplay.DebugDeleteSaveData.performed += ctx => DeleteSaveData();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		private void DeleteSaveData()
		{
			ProgressHandler progHandler = FindObjectOfType<ProgressHandler>();
			progHandler.WipeProgress();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}

