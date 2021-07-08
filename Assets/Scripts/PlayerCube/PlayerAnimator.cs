using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float altWiggleDelay = 1f;
		//Cache
		Animator animator;

		//States
		Vector3 playerLandPos;
		bool fallen = false;

		//Actions, events, delegates etc
		public event Action onTriggerLandingReaction;
		public event Action onTriggerShapieShock;
		public event Action onTriggerSerpent;

		public delegate Vector3 FinishPosDel();
		public FinishPosDel onGetFinishPos;

		public delegate bool SegOrShapeDel();
		public SegOrShapeDel onHasSeg;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		private void Start() 
		{
			playerLandPos = onGetFinishPos();
		}

		private void TriggerLandingReaction() //Called from animation event
		{
			onTriggerLandingReaction();
		}

		public void TriggerFalling(float addedY)
		{
			StartCoroutine(Fall(addedY));
		}

		private IEnumerator Fall(float addedY)
		{
			fallen = true;

			GameObject player = this.gameObject;
			player.transform.position = new Vector3(playerLandPos.x,
				playerLandPos.y + addedY, playerLandPos.z);

			player.transform.rotation = Quaternion.Euler(0f, -45f, 0f);

			yield return null; //This is to prevent cube from showing for 1 frame at wrong loc

			EnableVisuals();

			animator.SetTrigger("Fall");
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

		private IEnumerator TriggerWiggleAlt()
		{
			yield return new WaitForSeconds(altWiggleDelay);
			if (!onHasSeg()) animator.SetTrigger("Wiggle");
		}

		public void TriggerLookDown()
		{
			animator.SetTrigger("LookDown");
		}

		public void TriggerWiggle()
		{
			animator.SetTrigger("Wiggle");
		}

		private void ActivateSerpent()
		{
			onTriggerSerpent(); 
			//TO DO: activate straight to serpent instead of via finish?
		}
	}
}
