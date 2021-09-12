using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qbism.Cubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
using Qbism.SceneTransition;


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

		//States
		Vector2 stickValue;
		bool inputting = false;

		private void Awake()
		{
			handler = GetComponent<CubeHandler>();
			mover = FindObjectOfType<PlayerCubeMover>();
			loader = GetComponent<SceneHandler>();
			rewinder = GetComponent<RewindHandler>();
			controls = new GameControls();

			//from Brackeys gamepad setup tut. Don't actually understand Lambda Expressions
			controls.Gameplay.Movement.performed += ctx => stickValue = ctx.ReadValue<Vector2>();
			controls.Gameplay.Rewind.performed += ctx => Rewind();
			controls.Gameplay.Restart.performed += ctx => RestartLevel();
			controls.Gameplay.DebugCompleteLevel.performed += ctx => FinishLevel();
		}
		
		private void OnEnable() 
		{
			controls.Gameplay.Enable();	
		}

		void Update()
		{	
			if(stickValue.x > -.1 && stickValue.x < .1 && 
				stickValue.y > -.1 && stickValue.y < .1) 
				inputting = false;

			if(!inputting) HandleStickValues();
		}

		private void HandleStickValues()
		{
			if((stickValue.x > .1 && stickValue.y > .1) ||
				(stickValue.x > -.05 && stickValue.x < .05 && stickValue.y > .5)) MoveUp();

			if((stickValue.x < -.1 && stickValue.y < -.1) ||
				(stickValue.x > -.05 && stickValue.x < .05 && stickValue.y < -.5)) MoveDown();

			if((stickValue.x < -.1 && stickValue.y > .1) ||
				(stickValue.y > -.05 && stickValue.y < .05 && stickValue.x < -.5)) MoveLeft();

			if((stickValue.x > .1 && stickValue.y < -.1) ||
				(stickValue.y > -.05 && stickValue.y < .05 && stickValue.x > .5)) MoveRight();
		}

		private void MoveUp()
		{
			if (mover.isOutOfBounds) return;

			inputting = true;
			var posAhead = mover.FetchGridPos() + Vector2Int.up;
			
			if (mover.isStunned || mover.isLowered) mover.InitiateWiggle(mover.up, Vector3.right);

			else if (handler.floorCubeDic.ContainsKey(posAhead))
				mover.HandleKeyInput(mover.up, Vector3.right, posAhead);

			else mover.InitiateWiggle(mover.up, Vector3.right);
		}

		private void MoveDown()
		{
			if (mover.isOutOfBounds) return;

			inputting = true;
			var posAhead = mover.FetchGridPos() + Vector2Int.down;

			if (mover.isStunned || mover.isLowered) mover.InitiateWiggle(mover.down, Vector3.left);

			else if (handler.floorCubeDic.ContainsKey(posAhead))
				mover.HandleKeyInput(mover.down, Vector3.left, posAhead);

			else mover.InitiateWiggle(mover.down, Vector3.left);
		}

		private void MoveLeft()
		{
			if (mover.isOutOfBounds) return;

			inputting = true;
			var posAhead = mover.FetchGridPos() + Vector2Int.left;

			if (mover.isStunned || mover.isLowered) mover.InitiateWiggle(mover.left, Vector3.forward);

			else if (handler.floorCubeDic.ContainsKey(posAhead))
				mover.HandleKeyInput(mover.left, Vector3.forward, posAhead);

			else mover.InitiateWiggle(mover.left, Vector3.forward);
		}

		private void MoveRight()
		{
			if (mover.isOutOfBounds) return;

			inputting = true;
			var posAhead = mover.FetchGridPos() + Vector2Int.right;

			if (mover.isStunned || mover.isLowered) mover.InitiateWiggle(mover.right, Vector3.back);

			else if (handler.floorCubeDic.ContainsKey(posAhead))
				mover.HandleKeyInput(mover.right, Vector3.back, posAhead);
				
			else mover.InitiateWiggle(mover.right, Vector3.back);
		}

		private void FinishLevel()
		{
			FinishCube finish = FindObjectOfType<FinishCube>();
			if(finish) finish.Finish();
		}
			
		private void Rewind()
		{
			rewinder.StartRewinding();
		}

		private void RestartLevel()
		{
			loader.RestartLevel();
		}

		private void OnDisable() 
		{
			controls.Gameplay.Disable();	
		}
	}
}
