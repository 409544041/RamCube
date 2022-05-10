using Qbism.Control;
using Qbism.PlayerCube;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
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
				if (cubeUI != null) cubeUI.showCubeUI = true;
			}
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
			if (mover.isOutOfBounds || !mover.input) return;

			inputDetector.inputting = true;
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

		public void HandleResetInput()
		{
			glRef.sceneHandler.RestartLevel();
		}

		public void HandleRewindInput()
		{
			glRef.rewindHandler.StartRewinding();
		}

		public void HandleEscapeInput()
		{
			stateMngr.SwitchState(stateMngr.pauseOverlayState, ScreenStates.pauseOverlayState);
		}

		public void HandleDebugCompleteInput()
		{
			if (gcRef.persRef.switchBoard.allowDebugFinish && mover.input) 
				gcRef.finishRef.finishCube.Finish();
		}

		public void StateExit()
		{
			foreach (var cubeRef in gcRef.cubeRefs)
			{
				var cubeUI = cubeRef.cubeUI;
				if (cubeUI != null) cubeUI.showCubeUI = false;
			}
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
		public void HandleDebugCompleteAllInput()
		{
		}
		public void HandleAnyInput()
		{ }
	}

}