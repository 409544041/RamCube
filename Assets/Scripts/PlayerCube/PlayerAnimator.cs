using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float altWiggleDelay = 1f, introDelay = 1f, fallDelay = .5f;
		[SerializeField] BoxCollider coll = null;
		[SerializeField] float impactGroundY, impactSegY;

		//Cache
		Animator animator;
		PlayerIntroJuicer introJuicer;
		PlayerOutroJuicer outroJuicer;

		//States
		Vector3 playerFinishLandPos;
		bool fallen = false;

		//Actions, events, delegates etc
		public event Action onTriggerLandingReaction;
		public event Action onShowFF, onTriggerSerpent, onChildSegmentToPlayer;
		public event Action<bool> onInputSet, onIntroSet, onSwitchVisuals;

		public delegate Vector3 FinishPosDel();
		public FinishPosDel onGetFinishPos;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
			introJuicer = GetComponentInParent<PlayerIntroJuicer>();
			outroJuicer = GetComponentInParent<PlayerOutroJuicer>();
		}

		private void Start() 
		{
			onSwitchVisuals(false);
			playerFinishLandPos = onGetFinishPos();
			StartCoroutine(DisableInputForDrop());
		}

		private void TriggerLandingReaction() //Called from animation event
		{
			onTriggerLandingReaction();
		}

		private IEnumerator DisableInputForDrop()
		{
			onIntroSet(true);
			onInputSet(false);

			animator.speed = 0; //pauze intro animation
			yield return new WaitForSeconds(introDelay);
			animator.speed = 1;
			onSwitchVisuals(true);

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
			GetComponentInParent<PlayerFartLauncher>().flyingBy = false;

			var mover = GetComponentInParent<PlayerCubeMover>();
			GameObject player = mover.gameObject;
			
			player.transform.position = new Vector3(playerFinishLandPos.x,
				playerFinishLandPos.y + addedY, playerFinishLandPos.z);

			player.transform.rotation = Quaternion.Euler(0f, fallDeg, 0f);

			//visuals were switched off in playerfartlauncher
			onSwitchVisuals(true);

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

		private void ActivateSerpent() //Called from animation
		{
			onChildSegmentToPlayer();
			coll.size = new Vector3(coll.size.x, 3, coll.size.z);
			
			onTriggerSerpent(); 
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
	}
}
