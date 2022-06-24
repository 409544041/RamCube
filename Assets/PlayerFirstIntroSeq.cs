using MoreMountains.Feedbacks;
using Qbism.Cubes;
using Qbism.Dialogue;
using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerFirstIntroSeq : MonoBehaviour
	{
		//Config parameters
		[SerializeField] InGameDialogueTrigger dialogueTrigger;
		[SerializeField] int flipsToStartPos;
		[SerializeField] FloorCube[] extraFloorCubes;
		[SerializeField] Vector3 startPos, camCenterStartPos;
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
			mover = pRef.playerMover;
			completed = E_LevelGameplayData.GetEntity(0).f_Completed;
			camOriginalSize = glRef.gcRef.gameCam.m_Lens.OrthographicSize;
			MMFlipVoice = playerFlipJuice.GetComponent<MMFeedbackSound>();
			MMFlipThud = playerPostFlipJuice.GetComponent<MMFeedbackSound>();
			sfxOriginalVolume = MMFlipVoice.MaxVolume;

			if (completed) return;

			glRef.gcRef.gameCam.m_Lens.OrthographicSize = camStartSize;
			pRef.animator.SetBool("IntroDrop", false);
			pRef.playerAnim.introDrop = false;

			pRef.transform.position = startPos;
		}

		private void Start()
		{
			if (completed)
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

			StartCoroutine(MoveToStartPos());
			StartCoroutine(ZoomCamToOriginalSize());
		}

		private IEnumerator MoveToStartPos()
		{
			var posAheadDir = Vector2Int.up;

			yield return new WaitForSeconds(entryDelay);

			for (int i = 0; i < flipsToStartPos; i++)
			{
				var posAhead = pRef.cubePos.FetchGridPos() + posAheadDir;

				yield return mover.Move(mover.up, Vector3.right, posAhead);
				yield return new WaitForSeconds(flipDelay);
			}

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
	}
}
