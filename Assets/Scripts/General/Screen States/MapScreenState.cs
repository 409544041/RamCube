using Qbism.Control;
using Qbism.Saving;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class MapScreenState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		MapCoreRefHolder mcRef;
		MapLogicRefHolder mlRef;
		PersistentRefHolder persRef;

		//States
		public bool allowInput { get; set; } = true;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				mcRef = stateMngr.mcRef;
				mlRef = mcRef.mlRef;
				persRef = mcRef.persRef;
			}
		}

		public void HandleActionInput()
		{
			if (!allowInput) return;

			mlRef.pinTracker.selectedPin.pinUI.LoadAssignedLevel();
		}

		public void HandleRewindInput()
		{
			if (!allowInput) return;

			if (persRef.switchBoard.serpentScreenConnected)
			{
				mlRef.serpentLoader.StartLoadingSerpentScreen();
			}
		}

		public void HandleResetInput()
		{
			if (!allowInput) return;

			if (persRef.switchBoard.allowDebugMapReload)
			{
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		public void HandleEscapeInput()
		{
			if (!allowInput) return;

			stateMngr.SwitchState(stateMngr.pauseOverlayState);
		}

		public void HandleDebugUnlockAllInput()
		{
			if (!allowInput) return;

			if (persRef.switchBoard.allowDebugUnlockAll)
			{
				mlRef.debugCompleter.UnlockAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		public void HandleDebugDeleteProgressInput()
		{
			if (!allowInput) return;

			if (persRef.switchBoard.allowDebugDeleteProgress)
			{
				mlRef.debugCompleter.UnCompleteAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		public void HandleDebugCompleteAllInput()
		{
			if (!allowInput) return;

			if (persRef.switchBoard.allowDebugCompleteAll)
			{
				mlRef.debugCompleter.CompleteAll();
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
		}

		public void StateExit()
		{
		}

		public void HandleAnyInput()
		{
		}
		public void HandleDebugCompleteInput()
		{
		}
		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
		}
	}
}
