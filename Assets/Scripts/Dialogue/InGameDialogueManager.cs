using Febucci.UI;
using MoreMountains.Feedbacks;
using Qbism.ScreenStateMachine;
using Qbism.Serpent;
using Qbism.SpriteAnimations;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Qbism.Dialogue
{
	public class InGameDialogueManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Vector3 floatingHeadPos;
		[SerializeField] float floatingHeadScale = 2.5f, firstTextDelay = .25f;
		[SerializeField] CanvasGroup dialogueCanvasGroup;
		[SerializeField] MMFeedbacks nextButtonJuice, textAppearJuice;
		[SerializeField] TextMeshProUGUI dialogueText, characterText;
		[SerializeField] TextAnimatorPlayer textAnimator;
		[SerializeField] GameLogicRefHolder glRef;
		[SerializeField] MapLogicRefHolder mlRef;

		//Cache
		InGameDialogueScripOb dialogueSO;
		ExpressionHandler exprHandler;

		//States
		GameObject floatingHead;
		Vector3 headRot;
		int dialogueIndex;
		Camera cam;
		bool isTyping = false;
		List<Renderer> mRenders = new List<Renderer>();
		ScreenStateManager screenStateMngr;
		MapDialogueMoment mapDialMoment;
		LevelPinUI selectedPinUI;

		private void Awake()
		{
			if (glRef != null)
			{
				cam = glRef.gcRef.cam;
				screenStateMngr = glRef.screenStateMngr;
			}
			else if (mlRef != null)
			{
				cam = mlRef.mcRef.cam;
				screenStateMngr = mlRef.screenStateMngr;
			}
		}

		public void StartInGameDialogue(InGameDialogueScripOb incDialogueSO, 
			MapDialogueMoment dialMoment, LevelPinUI selPinUI)
		{
			screenStateMngr.SwitchState(screenStateMngr.dialogueOverlayState, 
				ScreenStates.dialogueOverlayState);

			if (dialMoment != null) mapDialMoment = dialMoment;
			else mapDialMoment = null;
			if (selPinUI != null) selectedPinUI = selPinUI;
			else selectedPinUI = null;

			dialogueSO = incDialogueSO;
			nextButtonJuice.Initialization();
			textAppearJuice.Initialization();
			dialogueIndex = 0;
			textAppearJuice.PlayFeedbacks();
			dialogueCanvasGroup.alpha = 1;
			if (glRef != null) glRef.gcRef.gameplayCanvasGroup.alpha = 0;
			if (mlRef != null) mlRef.mcRef.mapCanvasGroup.alpha = 0;

			var entity = E_Segments.FindEntity(entity =>
				entity.f_name == dialogueSO.character.ToString());
			var headPrefab = (GameObject)entity.f_Prefab;
			headRot = entity.f_InGameDialogueRotation;

			SpawnHead(headPrefab);
			var segRef = floatingHead.GetComponent<SegmentRefHolder>();

			if (segRef != null)
			{
				exprHandler = segRef.exprHandler;
				var meshRenders = segRef.meshes;
				var segEntity = segRef.mSegments;

				for (int i = 0; i < meshRenders.Length; i++)
				{
					mRenders.Add(meshRenders[i]);
				}

				for (int i = 0; i < mRenders.Count; i++)
				{
					for (int k = 0; k < mRenders[i].materials.Length; k++)
					{
						OverrideLightDir(mRenders[i].materials[k], segEntity);
					}
				}

				characterText.text = segRef.mSegments.f_SegmentName;
				if (segRef.dragonAnim != null) segRef.dragonAnim.DragonSmile();
			}

			StartCoroutine(Dialogue());
		}

		private IEnumerator Dialogue()
		{
			nextButtonJuice.StopFeedbacks();
			textAppearJuice.PlayFeedbacks();

			if (exprHandler != null)
				exprHandler.SetFace(dialogueSO.dialogues[dialogueIndex].expression, -1);

			if (dialogueIndex == 0)
			{
				dialogueText.text = " ";
				yield return new WaitForSeconds(firstTextDelay);
			}

			dialogueText.text = dialogueSO.dialogues[dialogueIndex].dialogueText;
		}

		public void NextDialogueText()
		{
			if (isTyping) textAnimator.SkipTypewriter();
			else
			{
				dialogueIndex++;
				if (dialogueIndex >= dialogueSO.dialogues.Length) ExitDialogue();
				else StartCoroutine(Dialogue());
			}
		}

		private void ExitDialogue()
		{
			nextButtonJuice.StopFeedbacks();
			GameObject.Destroy(floatingHead);
			dialogueCanvasGroup.alpha = 0;
			if (glRef != null) glRef.gcRef.gameplayCanvasGroup.alpha = 1;
			if (mlRef != null) mlRef.mcRef.mapCanvasGroup.alpha = 1;
			dialogueText.text = " ";

			HandlePostDialogueActions();

			if (glRef != null) screenStateMngr.SwitchState(screenStateMngr.levelScreenState,
				ScreenStates.levelScreenState);
			else if (mlRef != null) screenStateMngr.SwitchState(screenStateMngr.mapScreenState,
				ScreenStates.mapScreenState);
		}

		private void HandlePostDialogueActions()
		{
			if (mapDialMoment != null) mapDialMoment.PostDialogue();
			else if (selectedPinUI != null)
			{
				mlRef.pinTracker.SetLevelPinButtonsInteractable(true);
				mlRef.pinTracker.SelectPin(selectedPinUI);
			}
		}

		private void OverrideLightDir(Material mat, M_Segments segEntity)
		{
			var pitch = segEntity.f_InGameDialogueLightPitchYaw.x;
			var yaw = segEntity.f_InGameDialogueLightPitchYaw.y;
			var rot = Quaternion.Euler(yaw, pitch, 0);
			var pitchRad = Mathf.Rad2Deg * rot.x;
			var yawRad = Mathf.Rad2Deg * rot.y;
			var dir = new Vector4(Mathf.Sin(pitchRad) * Mathf.Sin(yawRad),
				Mathf.Cos(pitchRad), Mathf.Sin(pitchRad) * Mathf.Cos(yawRad), 0.0f);

			mat.SetFloat("_OverrideLightmapDir", 1);
			mat.SetVector("_LightmapDirection", dir);
			mat.SetFloat("_UnityShadowPower", 0);
		}

		private void SpawnHead(GameObject headPrefab)
		{
			var spawnPos = cam.ViewportToWorldPoint(floatingHeadPos);
			floatingHead = Instantiate(headPrefab, spawnPos, Quaternion.identity);
			floatingHead.transform.parent = cam.transform;
			floatingHead.transform.localRotation = 
				Quaternion.Euler(headRot.x, headRot.y, headRot.z);
			floatingHead.transform.localScale = transform.localScale * floatingHeadScale;
			var segRef = floatingHead.GetComponent<SegmentRefHolder>();
			segRef.dialoguePopInJuice.Initialization();
			segRef.dialoguePopInJuice.PlayFeedbacks();
		}

		public void PulseNextButton() //Called from TextAnimatorPlayer events
		{
			nextButtonJuice.PlayFeedbacks();
		}
	}
}
