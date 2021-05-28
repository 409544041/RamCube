﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class BigEyeAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float minClosed, maxClosed, minAction, maxAction, minBlink, maxBlink;

		//Cache
		Animator animator;

		//States
		bool counting = true, blinkCounting = true;
		float timeCounter = 0f, blinkCounter = 0f;
		bool isOpen = false;
		float closedTime, actionTime, blinkTime;
		string[] anims = new string[5];
		bool inAction = false, blinking = false;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			ResetActionTime(); //Don't change order of these resets as not to fudge up isOpen on start
			ResetClosedTime();
			ResetBlinkTime();
			PopulateAnimsArray();
		}

		private void Update()
		{
			if (counting) timeCounter += Time.deltaTime;
			if (blinkCounting) blinkCounter += Time.deltaTime;

			if (!isOpen && timeCounter >= closedTime) OpenEye();
			if (isOpen && timeCounter >= actionTime && !inAction) DoEyeAction();
			if (isOpen && blinkCounter >= blinkTime && !blinking) Blink();
		}

		private void OpenEye()
		{
			animator.SetTrigger("Open");
			counting = false;
		}

		private void DoEyeAction()
		{
			inAction = true;
			string actionString = anims[Random.Range(0, anims.Length)];
			animator.SetTrigger(actionString);
			counting = false;
		}

		private void Blink()
		{
			animator.SetLayerWeight(1, 1);
			animator.SetTrigger("Blink");
			blinking = true;
			blinkCounting = false;
		}

		private void ResetClosedTime() //Gets called by animation event
		{
			closedTime = Random.Range(minClosed, maxClosed);
			timeCounter = 0;
			counting = true;
			if (isOpen) isOpen = false;
		}

		private void ResetActionTime() //Gets called by animation event
		{
			actionTime = Random.Range(minAction, maxAction);
			timeCounter = 0;
			counting = true;
			inAction = false;
			if (!isOpen) isOpen = true;
		}

		private void ResetBlinkTime() //Gets called by animation event
		{
			blinkTime = Random.Range(minBlink, maxBlink);
			blinkCounter = 0;
			blinkCounting = true;
			blinking = false;
			animator.SetLayerWeight(1, 0);
		}

		private void PopulateAnimsArray()
		{
			anims[0] = "Close";
			anims[1] = "Look01";
			anims[2] = "Look01";
			anims[3] = "Look02";
			anims[4] = "Look02";
		}

		private void IsOpenTrue() //Gets called by animation event
		{
			isOpen = true;
		}

		private void IsOpenFalse() //Gets called by animation event
		{
			isOpen = false;
		}
	}
}
