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
			if (!fallen) StartCoroutine(Fall(addedY));
		}

		private IEnumerator Fall(float addedY)
		{
			fallen = true; 

			animator.SetTrigger("Fall");

			//to ensure the player isn't on screen for 1 frame before starting anim
			yield return null; 

			GameObject player = this.gameObject;
			player.transform.position = new Vector3(playerLandPos.x,
				playerLandPos.y + addedY, playerLandPos.z);

			player.transform.rotation = Quaternion.Euler(0f, -45f, 0f);
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
