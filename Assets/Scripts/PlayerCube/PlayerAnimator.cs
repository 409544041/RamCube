using Qbism.Serpent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float altWiggleDelay = 1f, introDelay = 1f, fallDelay = .5f, 
			faceDownWiggleDelay = .5f;
		[SerializeField] BoxCollider coll = null;
		[SerializeField] float impactGroundY, impactSegY;
		[SerializeField] PlayerRefHolder refs;

		//Cache
		Animator animator;
		PlayerIntroJuicer introJuicer;
		PlayerOutroJuicer outroJuicer;
		PlayerFartJuicer fartJuicer;
		SegmentRefHolder rescuedSegRef;

		//States
		Vector3 playerFinishLandPos;
		bool fallen = false;
		bool serpentActivated = false;

		//Actions, events, delegates etc
		public event Action onTriggerLandingReaction;
		public event Action onShowFF, onTriggerSerpent, onChildSegmentToPlayer;
		public event Action<bool> onInputSet, onIntroSet;

		public delegate Vector3 FinishPosDel();
		public FinishPosDel onGetFinishPos;

		private void Awake() 
		{
			animator = refs.animator;
			introJuicer = refs.introJuicer;
			outroJuicer = refs.outroJuicer;
			fartJuicer = refs.fartJuicer;
		}

		private void Start() 
		{
			refs.visualSwitch.SwitchMeshes(false);
			refs.visualSwitch.SwitchSprites(false);
			playerFinishLandPos = onGetFinishPos();
			StartCoroutine(DisableInputForDrop());
		}

		private void FindRescuedSegRef()
		{
			SegmentRefHolder[] segRefs = FindObjectsOfType<SegmentRefHolder>();
			foreach (var segRef in segRefs)
			{
				if (segRef.segAnim != null && segRef.segAnim.justSpawned)
					rescuedSegRef = segRef;
			}
		}

		private void TriggerLandingReaction() //Called from animation event
		{
			if (rescuedSegRef == null) FindRescuedSegRef();
			if (rescuedSegRef != null) rescuedSegRef.segAnim.TriggerSquish();
		}

		private IEnumerator DisableInputForDrop()
		{
			onIntroSet(true);
			onInputSet(false);

			animator.speed = 0; //pauze intro animation
			yield return new WaitForSeconds(introDelay);
			animator.speed = 1;
			refs.visualSwitch.SwitchMeshes(true);
			refs.visualSwitch.SwitchSprites(true);

			var currentClipinfo = animator.GetCurrentAnimatorClipInfo(0);
			float clipLength = currentClipinfo[0].clip.length;
			yield return new WaitForSeconds(clipLength);

			onInputSet(true);
			onIntroSet(false);
			onShowFF();
		}

		public void TriggerFall(bool hasSeg, string fallType, float fallDeg, bool withMom)
		{
			if (fallen) return;

			var impactY = impactGroundY;
			if (withMom) impactY = 0;

			if (hasSeg) StartCoroutine(Fall(impactSegY, fallType, fallDeg));
			else StartCoroutine(Fall(impactY,fallType, fallDeg));
		}

		private IEnumerator Fall(float addedY, string fallType, float fallDeg)
		{
			yield return new WaitForSeconds(fallDelay);

			fallen = true;
			refs.fartLauncher.flyingBy = false;
			
			refs.transform.position = new Vector3(playerFinishLandPos.x,
				playerFinishLandPos.y + addedY, playerFinishLandPos.z);

			refs.transform.rotation = Quaternion.Euler(0f, fallDeg, 0f);

			//visuals were switched off in playerfartlauncher
			refs.visualSwitch.SwitchMeshes(true);
			refs.visualSwitch.SwitchSprites(true);

			animator.SetTrigger(fallType);

			outroJuicer.PlayFallingSound();
		}

		private IEnumerator TriggerDelayedWiggle() //Called from animation
		{
			yield return new WaitForSeconds(altWiggleDelay);
			animator.SetTrigger("Wiggle");
		}

		public void TriggerLookDown() //Called from animation
		{
			animator.SetTrigger("LookDown");
		}

		public void TriggerWiggle() //Called from animation
		{
			animator.SetTrigger("Wiggle");
		}

		public void SetWithMother(bool value)
        {
			animator.SetBool("WithMother", value); 
        }

		public void TriggerCuddle()
        {
			animator.SetTrigger("Cuddle");
        }

		public void ActivateSerpent() //Called from animation
		{
			if (serpentActivated) return;

			if (rescuedSegRef == null) FindRescuedSegRef();
			if (rescuedSegRef != null) rescuedSegRef.segAnim.ChildToPlayer();

			coll.size = new Vector3(coll.size.x, 3, coll.size.z);
			
			onTriggerSerpent(); 
			serpentActivated = true;
			//TO DO: activate straight to serpent instead of via finish?
		}

		private void TriggerIntroSpeedJuice() //Called from animation
		{
			introJuicer.TriggerSpeedJuice();
		}

		private void TriggerIntroLandingJuice() //Called from animation
		{
			introJuicer.TriggerIntroLandingJuice();
		}

		private void TriggerButtPopVFX() //Called from animation
		{
			introJuicer.PlayButtPopFX();
		}

		private void TriggerPopVFX() //Called from animation
		{
			introJuicer.PlayPopVFX();
		}

		private void PlayEndLevelLandingSound() //Called from animation
		{
			outroJuicer.PlayLandingSound();
		}

		private void PlaySecondLendingSound() //Called from animation
		{
			outroJuicer.PlaySmallLandingSound();
		}

		private void PlaySurpriseSound() //Called from animation
		{
			outroJuicer.PlaySurpriseSound();
		}

		private void PlayEndLaughSount() //Called from animation
		{
			outroJuicer.PlayEndLaughSound();
		}

		private void PlayToothyLaughSound() //Called from animation
		{
			outroJuicer.PlayToothyLaughSound();
		}

		private void PlaySwallowOrOuchSound() //Called from animation
		{
			if (fartJuicer.progHandler.currentHasObject) outroJuicer.PlaySwallowClip();
			else outroJuicer.PlayOuchSound();
		}

		private void PlayOutchSound() // called from animation
		{
			outroJuicer.PlayOuchSound();
		}

		private void TriggerSputterFarts() // called from animation
		{
			fartJuicer.TriggerSputterFarts();
		}

		private void TriggerObjectFart()
		{
			fartJuicer.InitiateObjectFart();
		}

		private IEnumerator TriggerFaceDownWiggle()
		{
			yield return new WaitForSeconds(faceDownWiggleDelay);
			animator.SetTrigger("Wiggle");
		}
	}
}
