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
		MapLogicRefHolder mlRef;
		PersistentRefHolder persRef;

		//States
		public bool allowInput { get; set; } = true;

		void Awake()
		{
			controls = new GameControls();

			controls.Gameplay.DebugKey4.performed += ctx => LoadSerpentScreen();
			controls.Gameplay.DebugKey2.performed += ctx => UnCompleteAllAndReload();
			controls.Gameplay.DebugKey3.performed += ctx => CompleteAllAndReload();
			controls.Gameplay.DebugKey1.performed += ctx => UnlockAllAndReload();
			controls.Gameplay.ZKey.performed += ctx => ReloadMap();
			controls.Gameplay.EscKey.performed += ctx => ShowQuitOverlay();
			controls.Gameplay.EnterKey.performed += ctx => EnterLevel();
			controls.Gameplay.SpaceKey.performed += ctx => EnterLevel();
			controls.Gameplay.XKey.performed += ctx => EnterLevel();

			mlRef = mcRef.mlRef;
			persRef = mcRef.persRef;
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		private void EnterLevel()
		{
			if (!allowInput) return;

			if (!mcRef.quitOverlay.overlayActive)
				mlRef.pinTracker.selectedPin.pinUI.LoadAssignedLevel();
			else Application.Quit();
		}

		private void LoadSerpentScreen()
		{
			if (!allowInput) return;
			if (mcRef.quitOverlay.overlayActive) return;

			if (persRef.switchBoard.serpentScreenConnected)
			{
				mlRef.serpentLoader.StartLoadingSerpentScreen();
			}
		}

		private void ReloadMap()
		{
			if (!allowInput) return;
			if (mcRef.quitOverlay.overlayActive) return;

			if (persRef.switchBoard.allowDebugMapReload)
			{
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void UnCompleteAllAndReload()
		{
			if (!allowInput) return;
			if (mcRef.quitOverlay.overlayActive) return;

			if (persRef.switchBoard.allowDebugDeleteProgress)
			{
				mlRef.debugCompleter.UnCompleteAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void CompleteAllAndReload()
		{
			if (!allowInput) return;
			if (mcRef.quitOverlay.overlayActive) return;

			if (persRef.switchBoard.allowDebugCompleteAll)
			{
				mlRef.debugCompleter.CompleteAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void UnlockAllAndReload()
		{
			if (!allowInput) return;
			if (mcRef.quitOverlay.overlayActive) return;

			if (persRef.switchBoard.allowDebugUnlockAll)
			{
				mlRef.debugCompleter.UnlockAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		private void ShowQuitOverlay()
		{
			if (!allowInput) return;

			if (!mcRef.quitOverlay.overlayActive)
				mcRef.quitOverlay.ShowOverlay();
			else mcRef.quitOverlay.InitiateHideOverlay();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}

