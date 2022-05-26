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

		public void HandleDebugToggleHudInput()
		{
			if (stateMngr.mcRef.mapCanvasGroup.alpha == 1)
			{
				stateMngr.mcRef.mapCanvasGroup.alpha = 0;
				stateMngr.mcRef.persRef.hudToggler.hudVisible = false;
			}
			else
			{
				stateMngr.mcRef.mapCanvasGroup.alpha = 1;
				stateMngr.mcRef.persRef.hudToggler.hudVisible = true;
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
		public void HandleResetInput()
		{
		}
	}
}
