using Qbism.Control;
using Qbism.Saving;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.ScreenStateMachine
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
		int stuffNotAllowingInput = 0;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				mcRef = stateMngr.mcRef;
				mlRef = mcRef.mlRef;
				persRef = mcRef.persRef;

				AddRemoveNotAllowingInput(1);
				mlRef.debugCompleter.CheckDebugStatuses();
				mlRef.pinChecker.CheckLevelPins();
			}
		}

		public void AddRemoveNotAllowingInput(int i)
		{
			stuffNotAllowingInput += i;
			if (stuffNotAllowingInput == 0)
			{
				allowInput = true;
				mlRef.pinTracker.SetLevelPinButtonsInteractable(true);
				mlRef.pinTracker.SelectPin(mlRef.pinTracker.selectedPin.pinUI);
			}
			else
			{
				allowInput = false;
				mlRef.pinTracker.SetLevelPinButtonsInteractable(false);
			}
		}

		public void HandleActionInput()
		{
			if (!allowInput) return;

			mlRef.pinTracker.selectedPin.pinUI.LoadAssignedLevel();
		}

		public void HandleBackInput()
		{
			if (!allowInput) return;
			if (!persRef.switchBoard.serpentScreenConnected) return;
			if (!E_SegmentsGameplayData.GetEntity(0).f_Rescued) return;
			
			mlRef.serpentLoader.StartLoadingSerpentScreen();
		}

		public void HandleEscapeInput()
		{
			if (!allowInput) return;

			stateMngr.SwitchState(stateMngr.pauseOverlayState, ScreenStates.pauseOverlayState);
		}

		public void HandleDebugCompleteInput()
		{
			if (!allowInput) return;

			if (persRef.switchBoard.allowDebugMapReload)
			{
				persRef.progHandler.SaveProgData();
				mlRef.mapLoader.StartLoadingWorldMap(false);
			}
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

		public void HandleResetInput()
		{
			if (!persRef.switchBoard.allowDebugMapReload) return;
			mlRef.mapLoader.StartLoadingWorldMap(false);
		}

		public void HandleDebugToggleHudInput()
		{
			if (!persRef.switchBoard.allowHudToggle) return;

			if (mcRef.mapCanvasGroup.alpha == 1)
			{
				mcRef.mapCanvasGroup.alpha = 0;
				mcRef.persRef.hudToggler.hudVisible = false;
			}
			else
			{
				mcRef.mapCanvasGroup.alpha = 1;
				mcRef.persRef.hudToggler.hudVisible = true;
			}
		}

		public void StateExit()
		{
		}

		public void HandleAnyInput()
		{
		}
		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
		}
		public void HandleRewindInput()
		{
		}
	}
}
