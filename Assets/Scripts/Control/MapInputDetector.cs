using System.Collections;
using System.Collections.Generic;
using Qbism.General;
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
		FeatureSwitchBoard switchBoard;

		void Awake()
		{
			progHandler = FindObjectOfType<ProgressHandler>();
			switchBoard = progHandler.GetComponent<FeatureSwitchBoard>();
			controls = new GameControls();

			// controls.Gameplay.DebugDeleteSaveData.performed += ctx => DeleteSaveData();
			controls.Gameplay.DebugKey4.performed += ctx => LoadSerpentScreen();
			controls.Gameplay.DebugKey2.performed += ctx => UnCompleteAllAndReload();
			controls.Gameplay.DebugKey3.performed += ctx => CompleteAllAndReload();
			controls.Gameplay.DebugKey1.performed += ctx => UnlockAllAndReload();
			controls.Gameplay.Rewind.performed += ctx => ReloadMap();
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
			if (switchBoard.serpentScreenConnected)
			{
				SceneHandler sceneHandler = GetComponent<SceneHandler>();
				sceneHandler.LoadSerpentScreen();
			}
		}

		private void ReloadMap()
		{
			if (switchBoard.allowDebugMapReload)
			{
				progHandler.SaveProgData();
				FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap(false);
			}
		}

		private void UnCompleteAllAndReload()
		{
			if (switchBoard.allowDebugDeleteProgress)
			{
				FindObjectOfType<MapDebugCompleter>().UnCompleteAll();
				progHandler.SaveProgData();
				FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap(false);
			}
		}

		private void CompleteAllAndReload()
		{
			if (switchBoard.allowDebugCompleteAll)
			{
				FindObjectOfType<MapDebugCompleter>().CompleteAll();
				progHandler.SaveProgData();
				FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap(false);
			}
		}

		private void UnlockAllAndReload()
		{
			if (switchBoard.allowDebugUnlockAll)
			{
				FindObjectOfType<MapDebugCompleter>().UnlockAll();
				progHandler.SaveProgData();
				FindObjectOfType<WorldMapLoading>().StartLoadingWorldMap(false);
			}
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}

