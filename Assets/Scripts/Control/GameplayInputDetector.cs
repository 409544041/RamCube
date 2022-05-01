using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qbism.Cubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
using Qbism.SceneTransition;
using Qbism.General;
using Qbism.Saving;
using Qbism.Dialogue;
using Qbism.Objects;

namespace Qbism.Control
{
	public class GameplayInputDetector : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameplayCoreRefHolder gcRef;
		[SerializeField] GameLogicRefHolder glRef;

		//Cache
		GameControls controls;
		PlayerRefHolder player;
		PlayerCubeMover mover;
		FeatureSwitchBoard switchBoard;
		FinishCube finish;
		OverlayMenuHandler levelExitOverlay;

		//States
		Vector2 stickValue;
		bool inputting = false;

		private void Awake()
		{
			player = gcRef.pRef;
			mover = player.playerMover;
			switchBoard = gcRef.persRef.switchBoard;
			finish = gcRef.finishRef.finishCube;
			levelExitOverlay = gcRef.exitOverlayHandler;
			controls = new GameControls();

			controls.Gameplay.Movement.performed += ctx => stickValue = ctx.ReadValue<Vector2>();
			controls.Gameplay.ZKey.performed += ctx => Rewind();
			controls.Gameplay.RKey.performed += ctx => HandleRestartInput();
			controls.Gameplay.EscKey.performed += ctx => BackToMap();
			controls.Gameplay.CKey.performed += ctx => FinishLevel();
			controls.Gameplay.XKey.performed += ctx => ContinueDialogue();
			controls.Gameplay.EnterKey.performed += ctx => ContinueDialogue();
			controls.Gameplay.SpaceKey.performed += ctx => ContinueDialogue();

		}

		private void OnEnable() 
		{
			controls.Gameplay.Enable();	
		}

		void Update()
		{
			//inputting is to avoid multiple movement inputs at once
			if (stickValue.x > -.1 && stickValue.x < .1 && 
				stickValue.y > -.1 && stickValue.y < .1) 
				inputting = false;

			if (!inputting) HandleStickValues();
		}

		private void HandleStickValues()
		{
			if ((stickValue.x > .1 && stickValue.y > .1) ||
				(stickValue.x > -.05 && stickValue.x < .05 && stickValue.y > .5))
				HandleMove(mover.up, Vector2Int.up, Vector3.right);

			if ((stickValue.x < -.1 && stickValue.y < -.1) ||
				(stickValue.x > -.05 && stickValue.x < .05 && stickValue.y < -.5))
				HandleMove(mover.down, Vector2Int.down, Vector3.left);

			if ((stickValue.x < -.1 && stickValue.y > .1) ||
				(stickValue.y > -.05 && stickValue.y < .05 && stickValue.x < -.5))
				HandleMove(mover.left, Vector2Int.left, Vector3.forward);

			if ((stickValue.x > .1 && stickValue.y < -.1) ||
				(stickValue.y > -.05 && stickValue.y < .05 && stickValue.x > .5))
				HandleMove(mover.right, Vector2Int.right, Vector3.back);
		}

		private void HandleMove(Transform turnSide, Vector2Int posAheadDir, Vector3 turnAxis)
		{
			if (mover.isOutOfBounds || !mover.input) return;

			inputting = true;
			var posAhead = player.cubePos.FetchGridPos() + posAheadDir;

			if (mover.isStunned || mover.isLowered) mover.InitiateWiggle(turnSide, turnAxis);

			else if (glRef.cubeHandler.floorCubeDic.ContainsKey(posAhead) ||
				glRef.cubeHandler.movFloorCubeDic.ContainsKey(posAhead))
			{
				if (mover.CheckForWallAhead(posAhead))
					mover.InitiateWiggle(turnSide, turnAxis);
				else mover.HandleKeyInput(turnSide, turnAxis, posAhead);
			}

			else mover.InitiateWiggle(turnSide, turnAxis);
		}

		private void FinishLevel()
		{	
			if (switchBoard.allowDebugFinish && mover.input) finish.Finish();
		}
			
		private void Rewind()
		{
			if (!finish.hasFinished && !levelExitOverlay.overlayActive)
				glRef.rewindHandler.StartRewinding();
		}

		private void HandleRestartInput()
		{
			if (!finish.hasFinished && !levelExitOverlay.overlayActive)
				glRef.sceneHandler.RestartLevel();
		}

		private void ContinueDialogue()
		{
			if (glRef.dialogueManager.inDialogue) glRef.dialogueManager.NextDialogueText();
			else if (glRef.objColManager.overlayActive) glRef.mapLoader.StartLoadingWorldMap(true);
			if (levelExitOverlay.overlayActive) glRef.mapLoader.StartLoadingWorldMap(true);
		}

		private void BackToMap()
		{
			if (finish.hasFinished) return;

			if (!levelExitOverlay.overlayActive) levelExitOverlay.ShowOverlay();
			else levelExitOverlay.InitiateHideOverlay(); ;
		}

		private void OnDisable() 
		{
			controls.Gameplay.Disable();	
		}
	}
}
