using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using Qbism.Serpent;
using UnityEngine;

namespace Qbism.Control
{
	public class SerpentScreenInputDetector : MonoBehaviour
	{
		//Config parameters
		[SerializeField] SerpCoreRefHolder scRef;

		//Cache
		GameControls controls;

		//States
		Vector2 stickValue;
		bool inputting = false;
		public bool allowInput { get; set; } = true;

		private void Awake() 
		{
			controls = new GameControls();
			controls.Gameplay.Movement.performed += ctx => stickValue = ctx.ReadValue<Vector2>();
			controls.Gameplay.EscKey.performed += ctx => LoadWorldMap();
			controls.Gameplay.XKey.performed += ctx => StartSerpScreenDialogue();
			controls.Gameplay.EnterKey.performed += ctx => StartSerpScreenDialogue();
			controls.Gameplay.SpaceKey.performed += ctx => StartSerpScreenDialogue();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		void Update()
		{
			if (!allowInput) return;

			//inputting is to avoid multiple movement inputs at once
			if (stickValue.x > -.1 && stickValue.x < .1 &&
				stickValue.y > -.1 && stickValue.y < .1)
				inputting = false;

			if (!inputting) HandleStickValues();
		}

		private void HandleStickValues()
        {
			if (stickValue.x > .5)
			{
				scRef.slRef.scroller.ScrollLeft();
				inputting = true;
			}
			else if (stickValue.x < -.5)
			{
				scRef.slRef.scroller.ScrollRight();
				inputting = true;
			}
		}

		private void LoadWorldMap()
		{
			scRef.slRef.mapLoader.StartLoadingWorldMap(false);
		}

		private void StartSerpScreenDialogue()
		{
			if (scRef.slRef.dialogueManager.inDialogue) 
				scRef.slRef.dialogueManager.NextDialogueText();
			else scRef.slRef.objSegChecker.StartDialogue();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}
