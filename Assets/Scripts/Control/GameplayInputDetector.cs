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

namespace Qbism.Control
{
	public class GameplayInputDetector : MonoBehaviour
	{
		//Cache
		CubeHandler handler;
		PlayerCubeMover mover;
		SceneHandler loader;
		RewindHandler rewinder;
		GameControls controls;
		FeatureSwitchBoard switchBoard;
		ProgressHandler progHandler;
		DialogueManager dialogueManager;

		//States
		Vector2 stickValue;
		bool inputting = false;

		private void Awake()
		{
			handler = GetComponent<CubeHandler>();
			mover = FindObjectOfType<PlayerCubeMover>();
			loader = GetComponent<SceneHandler>();
			rewinder = GetComponent<RewindHandler>();
			progHandler = FindObjectOfType<ProgressHandler>();
			switchBoard = progHandler.GetComponent<FeatureSwitchBoard>();
			dialogueManager = GetComponent<DialogueManager>();
			controls = new GameControls();

			controls.Gameplay.Movement.performed += ctx => stickValue = ctx.ReadValue<Vector2>();
			controls.Gameplay.Rewind.performed += ctx => Rewind();
			controls.Gameplay.Restart.performed += ctx => HandleRestartInput();
			controls.Gameplay.DebugCompleteLevel.performed += ctx => FinishLevel();
			controls.Gameplay.DebugNextLevel.performed += ctx  => NextLevel();
			controls.Gameplay.DebugPrevLevel.performed += ctx => PrevLevel();
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
			if (mover.isOutOfBounds) return;

			inputting = true;
			var posAhead = mover.cubePoser.FetchGridPos() + posAheadDir;

			if (mover.isStunned || mover.isLowered) mover.InitiateWiggle(turnSide, turnAxis);

			else if (handler.floorCubeDic.ContainsKey(posAhead) || 
				handler.movFloorCubeDic.ContainsKey(posAhead))
			{
				if (mover.CheckForWallAhead(posAhead))
					mover.InitiateWiggle(turnSide, turnAxis);
				else mover.HandleKeyInput(turnSide, turnAxis, posAhead);
			}

			else mover.InitiateWiggle(turnSide, turnAxis);
		}

		private void NextLevel()
		{
			if (switchBoard.allowDebugLevelNav)
				loader.NextLevel();
		}

		private void PrevLevel()
		{
			if (switchBoard.allowDebugLevelNav)
				loader.PreviousLevel();
		}

		private void FinishLevel()
		{	
			if (switchBoard.allowDebugFinish)
			{
				FinishCube finish = FindObjectOfType<FinishCube>();
				if (finish) finish.Finish();
			}
		}
			
		private void Rewind()
		{
			rewinder.StartRewinding();
		}

		private void HandleRestartInput()
		{
			if (dialogueManager.inDialogue) dialogueManager.NextDialogueText();
			else loader.RestartLevel();
		}

		private void OnDisable() 
		{
			controls.Gameplay.Disable();	
		}
	}
}
