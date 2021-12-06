using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qbism.Control
{
	public class MapInputDetector : MonoBehaviour
	{
		//Cache
		GameControls controls;
		ProgressHandler progHandler;

		void Awake()
		{
			controls = new GameControls();

			controls.Gameplay.DebugDeleteSaveData.performed += ctx => DeleteSaveData();
			controls.Gameplay.DebugKey4.performed += ctx => LoadSerpentScreen();
			controls.Gameplay.Restart.performed += ctx => ReloadScene();

			progHandler = FindObjectOfType<ProgressHandler>();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		private void DeleteSaveData()
		{
			progHandler.WipeProgData();
		}

		private void LoadSerpentScreen()
		{
			SceneHandler sceneHandler = GetComponent<SceneHandler>();
			sceneHandler.LoadSerpentScreen();
		}

		private void ReloadScene()
		{
			progHandler.SaveProgData();
			FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}

