using MoreMountains.Feedbacks;
using Qbism.Cubes;
using Qbism.Dialogue;
using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.PlayerCube
{
	public class PlayerFirstIntroSeq : MonoBehaviour
	{
		//Config parameters
		[SerializeField] InGameDialogueTrigger dialogueTrigger;
		[SerializeField] FloorCube[] extraFloorCubes;
		[SerializeField] FloorCube startCube, targetCube;
		[SerializeField] Vector3 playerStartRot;
		[SerializeField] Vector3 camCenterStartPos;
		[SerializeField]
		float flipDelay = .25f, delayAtStartPos = 1, entryDelay = .75f,
							camStartSize = 10, zoomDur = 10, sfxStartVolume = .2f;
		[SerializeField] AnimationCurve zoomCurve;
		[SerializeField] PositionCenterpoint camCenter;
		[SerializeField] MMFeedbacks playerFlipJuice, playerPostFlipJuice;
		[SerializeField] PlayerRefHolder pRef;
		[SerializeField] GameLogicRefHolder glRef;

		//Cache
		PlayerCubeMover mover;
		MMFeedbackSound MMFlipVoice, MMFlipThud;

		//States
		bool completed;
		float camOriginalSize;
		float sfxOriginalVolume;

		private void Awake()
		{
			completed = E_LevelGameplayData.GetEntity(0).f_Completed;
			if (completed || !glRef.gcRef.persRef.switchBoard.showFirstLevelIntroRoll) return;

			mover = pRef.playerMover;
			camOriginalSize = glRef.gcRef.gameCam.m_Lens.OrthographicSize;
			MMFlipVoice = playerFlipJuice.GetComponent<MMFeedbackSound>();
			MMFlipThud = playerPostFlipJuice.GetComponent<MMFeedbackSound>();
			sfxOriginalVolume = MMFlipVoice.MaxVolume;

			glRef.gcRef.gameCam.m_Lens.OrthographicSize = camStartSize;
			pRef.animator.SetBool("IntroDrop", false);
			pRef.playerAnim.introDrop = false;

			pRef.transform.position = new Vector3(startCube.transform.position.x,
				startCube.transform.position.y + 1, startCube.transform.position.z);
			pRef.transform.localRotation = Quaternion.Euler(playerStartRot);
			glRef.gcRef.persRef.progHandler.currentHasSegment = true;
		}

		private void Start()
		{
			if (completed || !glRef.gcRef.persRef.switchBoard.showFirstLevelIntroRoll)
			{
				foreach (var fCube in extraFloorCubes)
				{
					fCube.gameObject.SetActive(false);
					glRef.cubeHandler.floorCubeDic.Remove(fCube.refs.cubePos.FetchGridPos());
					camCenter.FindEdgeCubes();
				}
				return;
			}

			camCenter.enabled = false;
			camCenter.transform.position = camCenterStartPos;

			DeselectLevelSelectOption();

			StartCoroutine(MoveToStartPos());
			StartCoroutine(ZoomCamToOriginalSize());
		}

		private IEnumerator MoveToStartPos()
		{
			var posAheadDir = Vector2Int.up;
			var dist = Vector3.Distance(startCube.transform.position,
				targetCube.transform.position);

			yield return new WaitForSeconds(entryDelay);

			for (int i = 0; i < dist; i++)
			{
				var posAhead = pRef.cubePos.FetchGridPos() + posAheadDir;

				yield return mover.Move(mover.up, Vector3.right, posAhead);
				yield return new WaitForSeconds(flipDelay);
			}

			pRef.timeBody.rewindList.Clear();
			yield return new WaitForSeconds(delayAtStartPos);

			dialogueTrigger.TriggerInGameDialogue();
			camCenter.enabled = true;
		}

		private IEnumerator ZoomCamToOriginalSize()
		{
			var startSize = camStartSize;
			var sfxStartVol = sfxStartVolume;
			float elapsedTime = 0;

			while (!Mathf.Approximately(glRef.gcRef.gameCam.m_Lens.OrthographicSize, 
				camOriginalSize))
			{
				elapsedTime += Time.deltaTime;
				var percentageComplet = elapsedTime / zoomDur;

				glRef.gcRef.gameCam.m_Lens.OrthographicSize = 
					Mathf.Lerp(startSize, camOriginalSize, zoomCurve.Evaluate(percentageComplet));

				MMFlipVoice.MaxVolume = Mathf.Lerp(sfxStartVolume, sfxOriginalVolume,
					zoomCurve.Evaluate(percentageComplet));
				MMFlipVoice.MinVolume = Mathf.Lerp(sfxStartVolume, sfxOriginalVolume,
					zoomCurve.Evaluate(percentageComplet));
				MMFlipThud.MaxVolume = Mathf.Lerp(sfxStartVolume, sfxOriginalVolume,
					zoomCurve.Evaluate(percentageComplet));
				MMFlipThud.MinVolume = Mathf.Lerp(sfxStartVolume, sfxOriginalVolume,
					zoomCurve.Evaluate(percentageComplet));

				yield return null;
			}
		}

		private void DeselectLevelSelectOption()
		{
			var pauseOverlay = glRef.gcRef.pauseOverlayHandler;
			var levelSelectIndex = FetchLevelSelectIndex(pauseOverlay);

			if (levelSelectIndex == 99)
			{
				Debug.Log("Can't find level select index");
				return;
			}

			var levelSelectButton = pauseOverlay.buttonHandlers[levelSelectIndex];
			levelSelectButton.buttonText.color = pauseOverlay.inactiveTextColor;
			levelSelectButton.button.interactable = false;

			if (levelSelectIndex - 1 >= 0) SetButtonAboveNav(levelSelectIndex, pauseOverlay);
			if (levelSelectIndex + 1 < pauseOverlay.buttonHandlers.Length) 
				SetButtonBelowNav(levelSelectIndex, pauseOverlay);

			OverlayButtonHandler[] newButtonHandlerArray = 
				new OverlayButtonHandler[pauseOverlay.buttonHandlers.Length - 1];

			var j = 0;
			for (int i = 0; i < pauseOverlay.buttonHandlers.Length; i ++)
			{
				if (i == levelSelectIndex) continue;
				newButtonHandlerArray[j] = pauseOverlay.buttonHandlers[i];
				j++;
			}

			pauseOverlay.buttonHandlers = newButtonHandlerArray;
		}

		private int FetchLevelSelectIndex(OverlayMenuHandler pauseOverlay)
		{
			var index = 99;
			for (int i = 0; i < pauseOverlay.buttonHandlers.Length; i++)
			{
				if (pauseOverlay.buttonHandlers[i].label == "level select")
					index = i;
			}
			return index;
		}

		private void SetButtonAboveNav(int levelSelectIndex, OverlayMenuHandler pauseOverlay)
		{
			var buttonAboveIndex = levelSelectIndex - 1;
			var buttonAbove = pauseOverlay.buttonHandlers[buttonAboveIndex];
			Button selectUp = buttonAbove.button.navigation.selectOnUp.GetComponent<Button>();

			Navigation nav = new Navigation();
			nav.mode = Navigation.Mode.Explicit;
			nav.selectOnDown = pauseOverlay.buttonHandlers[levelSelectIndex + 1].button;
			nav.selectOnUp = selectUp;

			buttonAbove.button.navigation = nav;
		}

		private void SetButtonBelowNav(int levelSelectIndex, OverlayMenuHandler pauseOverlay)
		{
			var buttonBelowIndex = levelSelectIndex + 1;
			var buttonBelow = pauseOverlay.buttonHandlers[buttonBelowIndex];
			Button selectDown = buttonBelow.button.navigation.selectOnDown.GetComponent<Button>();

			Navigation nav = new Navigation();
			nav.mode = Navigation.Mode.Explicit;
			nav.selectOnUp = pauseOverlay.buttonHandlers[levelSelectIndex - 1].button;
			nav.selectOnDown = selectDown;

			buttonBelow.button.navigation = nav;
		}
	}
}
