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
		//Config parameters
		[SerializeField] MapCoreRefHolder mcRef;

		//Cache
		GameControls controls;

		//States
		MapLogicRefHolder mlRef;
		PersistentRefHolder persRef;

		void Awake()
		{
			controls = new GameControls();

			// controls.Gameplay.DebugDeleteSaveData.performed += ctx => DeleteSaveData();
			controls.Gameplay.DebugKey4.performed += ctx => LoadSerpentScreen();
			controls.Gameplay.DebugKey2.performed += ctx => UnCompleteAllAndReload();
			controls.Gameplay.DebugKey3.performed += ctx => CompleteAllAndReload();
			controls.Gameplay.DebugKey1.performed += ctx => UnlockAllAndReload();
			controls.Gameplay.Rewind.performed += ctx => ReloadMap();

			mlRef = mcRef.mlRef;
			persRef = mcRef.persRef;
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		private void DeleteSaveData()
		{
			persRef.progHandler.WipeProgData();
		}

		private void LoadSerpentScreen()
		{
			if (persRef.switchBoard.serpentScreenConnected)
			{
				mlRef.serpentLoader.StartLoadingSerpentScreen();
			}
		}

		private void ReloadMap()
		{
			if (persRef.switchBoard.allowDebugMapReload)
			{
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void UnCompleteAllAndReload()
		{
			if (persRef.switchBoard.allowDebugDeleteProgress)
			{
				mlRef.debugCompleter.UnCompleteAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void CompleteAllAndReload()
		{
			if (persRef.switchBoard.allowDebugCompleteAll)
			{
				mlRef.debugCompleter.CompleteAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void UnlockAllAndReload()
		{
			if (persRef.switchBoard.allowDebugUnlockAll)
			{
				mlRef.debugCompleter.UnlockAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}

