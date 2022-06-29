using Qbism.Serpent;
using Qbism.SpriteAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Dialogue
{
	public class InGameDialogueManager : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameLogicRefHolder glRef;
		[SerializeField] Vector3 floatingHeadPos;
		[SerializeField] float floatingHeadScale = 2.5f, firstTextDelay = .25f;

		//Cache
		GameplayCoreRefHolder gcRef;
		InGameDialogueScripOb dialogueSO;
		ExpressionHandler exprHandler;

		//States
		GameObject floatingHead;
		Vector3 headRot;
		int dialogueIndex;
		Camera cam;
		bool isTyping = false;
		List<Renderer> mRenders = new List<Renderer>();

		private void Awake()
		{
			gcRef = glRef.gcRef;
			cam = gcRef.cam;
		}

		public void StartInGameDialogue(InGameDialogueScripOb incDialogueSO)
		{
			glRef.screenStateMngr.SwitchState(glRef.screenStateMngr.dialogueOverlayState, 
				ScreenStates.dialogueOverlayState);

			dialogueSO = incDialogueSO;
			gcRef.inGameDialogueNextButtonJuice.Initialization();
			gcRef.inGameTextAppearJuice.Initialization();
			dialogueIndex = 0;
			gcRef.inGameTextAppearJuice.PlayFeedbacks();
			gcRef.inGameDialogueCanvasGroup.alpha = 1;
			gcRef.gameplayCanvasGroup.alpha = 0;

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

				gcRef.inGameCharacterNameText.text = segRef.mSegments.f_SegmentName;
				if (segRef.dragonAnim != null) segRef.dragonAnim.DragonSmile();
			}

			StartCoroutine(Dialogue());
		}

		private IEnumerator Dialogue()
		{
			gcRef.inGameDialogueNextButtonJuice.StopFeedbacks();
			gcRef.inGameTextAppearJuice.PlayFeedbacks();

			if (exprHandler != null)
				exprHandler.SetFace(dialogueSO.dialogues[dialogueIndex].expression, -1);

			if (dialogueIndex == 0)
			{
				gcRef.inGameDialogueText.text = " ";
				yield return new WaitForSeconds(firstTextDelay);
			}

			gcRef.inGameDialogueText.text = dialogueSO.dialogues[dialogueIndex].dialogueText;
		}

		public void NextDialogueText()
		{
			if (isTyping) gcRef.inGameTypeWriter.SkipTypewriter();
			else
			{
				dialogueIndex++;
				if (dialogueIndex >= dialogueSO.dialogues.Length) ExitDialogue();
				else StartCoroutine(Dialogue());
			}
		}

		private void ExitDialogue()
		{
			gcRef.inGameDialogueNextButtonJuice.StopFeedbacks();
			GameObject.Destroy(floatingHead);
			gcRef.inGameDialogueCanvasGroup.alpha = 0;
			gcRef.gameplayCanvasGroup.alpha = 1;
			gcRef.inGameDialogueText.text = " ";

			glRef.screenStateMngr.SwitchState(glRef.screenStateMngr.levelScreenState,
				ScreenStates.levelScreenState);
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
			gcRef.inGameDialogueNextButtonJuice.PlayFeedbacks();
		}
	}
}
