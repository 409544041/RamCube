using Qbism.Control;
using Qbism.General;
using Qbism.PlayerCube;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Qbism.ScreenStateMachine
{
	public class LevelScreenState : MonoBehaviour, IScreenBaseState
	{
		//Cache
		ScreenStateManager stateMngr;
		GameplayCoreRefHolder gcRef;
		GameLogicRefHolder glRef;
		PlayerCubeMover mover;

		public void StateEnter(ScreenStateManager ssm)
		{
			if (stateMngr == null)
			{
				stateMngr = ssm;
				gcRef = stateMngr.gcRef;
				glRef = gcRef.glRef;
				mover = gcRef.pRef.playerMover;
			}

			foreach (var cubeRef in gcRef.cubeRefs)
			{
				var cubeUI = cubeRef.cubeUI; 
				var floorCube = cubeRef.floorCube;
				if (cubeUI != null && floorCube != null) cubeUI.showCubeUI = true;
			}

			glRef.levelTimer.StartCountingTimer();
		}

		public void HandleStickValues(Vector2 stickValue, InputDetector inputDetector)
		{
			if ((stickValue.x > .1 && stickValue.y > .1) ||
				(stickValue.x > -.05 && stickValue.x < .05 && stickValue.y > .5))
				HandleMoveInput(mover.up, Vector2Int.up, Vector3.right, inputDetector);

			if ((stickValue.x < -.1 && stickValue.y < -.1) ||
				(stickValue.x > -.05 && stickValue.x < .05 && stickValue.y < -.5))
				HandleMoveInput(mover.down, Vector2Int.down, Vector3.left, inputDetector);

			if ((stickValue.x < -.1 && stickValue.y > .1) ||
				(stickValue.y > -.05 && stickValue.y < .05 && stickValue.x < -.5))
				HandleMoveInput(mover.left, Vector2Int.left, Vector3.forward, inputDetector);

			if ((stickValue.x > .1 && stickValue.y < -.1) ||
				(stickValue.y > -.05 && stickValue.y < .05 && stickValue.x > .5))
				HandleMoveInput(mover.right, Vector2Int.right, Vector3.back, inputDetector);
		}

		public void HandleMoveInput(Transform turnSide, Vector2Int posAheadDir, Vector3 turnAxis,
			InputDetector inputDetector)
		{
			if (mover.isOutOfBounds || !mover.initiatedByPlayer || !mover.allowMoveInput) return;

			inputDetector.inputting = true;
			if (mover.isMoving && !mover.newInput && !mover.isLowered)
			{
				mover.newInput = true;
				StartCoroutine(NextInput(turnSide, posAheadDir, turnAxis, inputDetector));
				return;
			}
			var posAhead = gcRef.pRef.cubePos.FetchGridPos() + posAheadDir;

			if (mover.isStunned || mover.isLowered) mover.InitiateWiggle(turnSide, turnAxis);

			else if (gcRef.glRef.cubeHandler.floorCubeDic.ContainsKey(posAhead) ||
				gcRef.glRef.cubeHandler.movFloorCubeDic.ContainsKey(posAhead))
			{
				if (mover.CheckForWallAhead(posAhead))
					mover.InitiateWiggle(turnSide, turnAxis);
				else mover.HandleKeyInput(turnSide, turnAxis, posAhead);
			}

			else mover.InitiateWiggle(turnSide, turnAxis);
		}

		private IEnumerator NextInput(Transform turnSide, Vector2Int posAheadDir, Vector3 turnAxis,
			InputDetector inputDetector)
		{
			while (mover.newInput) yield return null;
			mover.prevMoveNewInput = true;
			HandleMoveInput(turnSide, posAheadDir, turnAxis, inputDetector);
		}

		public void HandleResetInput()
		{
			SendRewindResetAnalyticsEvent();
			glRef.rewindHandler.ResetLevel();
		}

		public void HandleRewindInput()
		{
			SendRewindResetAnalyticsEvent();
			glRef.rewindHandler.StartRewinding();
		}

		public void HandleEscapeInput()
		{
			stateMngr.SwitchState(stateMngr.pauseOverlayState, ScreenStates.pauseOverlayState);
		}

		public void HandleDebugCompleteInput()
		{
			if (gcRef.persRef.switchBoard.allowDebugFinish && mover.allowMoveInput) 
				gcRef.finishRef.finishCube.Finish(true);
		}

		public void HandleDebugToggleHudInput()
		{
			if (!gcRef.persRef.switchBoard.allowHudToggle) return;

			if (gcRef.gameplayCanvasGroup.alpha == 1)
			{
				gcRef.gameplayCanvasGroup.alpha = 0;
				gcRef.persRef.hudToggler.hudVisible = false;
			}
			else
			{
				gcRef.gameplayCanvasGroup.alpha = 1;
				gcRef.persRef.hudToggler.hudVisible = true;
			}
		}

		public void HandleDebugCompleteAllInput()
		{
			gcRef.glRef.cubeHandler.ToggleCubeUI();
		}

		public void StateExit()
		{
			foreach (var cubeRef in gcRef.cubeRefs)
			{
				var cubeUI = cubeRef.cubeUI;
				if (cubeUI != null) cubeUI.showCubeUI = false;
			}

			glRef.levelTimer.StopCountingTimer();
		}

		private void SendRewindResetAnalyticsEvent()
		{
			Analytics.CustomEvent(AnalyticsEvents.RewindedReset, new Dictionary<string, object>()
			{
				{AnalyticsEvents.Level, gcRef.persRef.progHandler.debugCurrentPin},
			});
		}

		private void SendLevelExitAnalyticsEvent()
		{
			Analytics.CustomEvent(AnalyticsEvents.LevelExit, new Dictionary<string, object>()
			{
				{AnalyticsEvents.Level, gcRef.persRef.progHandler.debugCurrentPin},
			});
		}

		public void HandleActionInput()
		{
		}
		public void HandleDebugUnlockAllInput()
		{
		}
		public void HandleDebugDeleteProgressInput()
		{
		}

		public void HandleAnyInput()
		{ }

		public void HandleBackInput()
		{
		}
	}

}