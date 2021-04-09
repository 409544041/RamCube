using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.SceneTransition;
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
			controls.Gameplay.DebugSceneSwitch01.performed += ctx => LoadSerpentScreen();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		private void DeleteSaveData()
		{
			ProgressHandler progHandler = FindObjectOfType<ProgressHandler>();
			progHandler.WipeProgData();
		}

		private void LoadSerpentScreen()
		{
			SceneHandler sceneHandler = GetComponent<SceneHandler>();
			sceneHandler.LoadSerpentScreen();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}

