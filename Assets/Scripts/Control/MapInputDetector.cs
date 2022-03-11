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
		[SerializeField] MapCoreRefHolder mapCoreRef;

		//Cache
		GameControls controls;

		//States
		MapLogicRefHolder logicRef;
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

			logicRef = mapCoreRef.mapLogicRef;
			persRef = mapCoreRef.persistantRef;
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
				logicRef.serpentLoader.StartLoadingSerpentScreen();
			}
		}

		private void ReloadMap()
		{
			if (persRef.switchBoard.allowDebugMapReload)
			{
				persRef.progHandler.SaveProgData();
				logicRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void UnCompleteAllAndReload()
		{
			if (persRef.switchBoard.allowDebugDeleteProgress)
			{
				logicRef.debugCompleter.UnCompleteAll();
				persRef.progHandler.SaveProgData();
				logicRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void CompleteAllAndReload()
		{
			if (persRef.switchBoard.allowDebugCompleteAll)
			{
				logicRef.debugCompleter.CompleteAll();
				persRef.progHandler.SaveProgData();
				logicRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void UnlockAllAndReload()
		{
			if (persRef.switchBoard.allowDebugUnlockAll)
			{
				logicRef.debugCompleter.UnlockAll();
				persRef.progHandler.SaveProgData();
				logicRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}

