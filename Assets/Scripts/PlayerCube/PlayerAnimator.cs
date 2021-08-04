using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float altWiggleDelay = 1f, introDelay = 1f;
		[SerializeField] BoxCollider coll = null;

		//Cache
		Animator animator;

		//States
		Vector3 playerLandPos;
		bool fallen = false;

		//Actions, events, delegates etc
		public event Action onTriggerLandingReaction;
		public event Action onTriggerShapieShock;
		public event Action onTriggerSerpent;
		public event Action onChildSegmentToPlayer;
		public event Action<bool> onInputSet, onIntroSet;

		public delegate Vector3 FinishPosDel();
		public FinishPosDel onGetFinishPos;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		private void Start() 
		{
			playerLandPos = onGetFinishPos();
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

			var currentClipinfo = animator.GetCurrentAnimatorClipInfo(0);
			float clipLength = currentClipinfo[0].clip.length;
			yield return new WaitForSeconds(clipLength);

			onInputSet(true);
			onIntroSet(false);
		}

		public void TriggerFall(float addedY, string fallType)
		{
			if (!fallen) StartCoroutine(Fall(addedY, fallType));
		}

		private IEnumerator Fall(float addedY, string fallType)
		{
			fallen = true;

			var mover = GetComponentInParent<PlayerCubeMover>();
			GameObject player = mover.gameObject;
			
			player.transform.position = new Vector3(playerLandPos.x,
				playerLandPos.y + addedY, playerLandPos.z);

			player.transform.rotation = Quaternion.Euler(0f, -45f, 0f);

			yield return null; //This is to prevent cube from showing for 1 frame at wrong loc

			EnableVisuals();

			animator.SetTrigger(fallType);
		}

		private void EnableVisuals()
		{
			SkinnedMeshRenderer[] meshes = GetComponentsInChildren<SkinnedMeshRenderer>();

			foreach (var mesh in meshes)
			{
				mesh.enabled = true;
			}

			SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

			foreach (var sprite in sprites)
			{
				sprite.enabled = true;
			}
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

		private void ActivateSerpent() //Called from animation
		{
			onChildSegmentToPlayer();
			coll.size = new Vector3(coll.size.x, 3, coll.size.z);
			
			onTriggerSerpent(); 
			//TO DO: activate straight to serpent instead of via finish?
		}
	}
}
