using System.Collections;
using System.Collections.Generic;
using Qbism.Saving;
using Qbism.SceneTransition;
using Qbism.WorldMap;
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
			controls.Gameplay.Restart.performed += ctx => UnCompleteAllAndReload();
			// controls.Gameplay.DebugCompleteLevel.performed += ctx => CompleteAll();
			controls.Gameplay.DebugKeyZ.performed += ctx => UnlockAllAndReload();
			controls.Gameplay.Rewind.performed += ctx => ReloadMap();

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

		private void ReloadMap()
		{
			progHandler.SaveProgData();
			FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap();
		}

		private void UnCompleteAllAndReload()
		{
			FindObjectOfType<MapDebugCompleter>().UnCompleteAll();
			progHandler.SaveProgData();
			FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap();
		}

		private void CompleteAllAndReload()
		{
			FindObjectOfType<MapDebugCompleter>().CompleteAll();
			progHandler.SaveProgData();
			FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap();
		}

		private void UnlockAllAndReload()
		{
			FindObjectOfType<MapDebugCompleter>().UnlockAll();
			progHandler.SaveProgData();
			FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}

