using System.Collections;
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
		bool counting = true;
		float timeCounter = 0f;
		bool isOpen = false;
		float closedTime, actionTime;
		string[] anims = new string[5];
		bool inAction = false;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			ResetActionTime(); //Don't change order of these resets as not to fudge up isOpen on start
			ResetClosedTime();
			PopulateAnimsArray();
		}

		private void Update()
		{
			if (counting) timeCounter += Time.deltaTime;

			if (!isOpen && timeCounter >= closedTime) OpenEye();
			if (isOpen && timeCounter >= actionTime && !inAction) DoEyeAction();
		}

		private void OpenEye()
		{
			animator.SetTrigger("Open");
			counting = false;
		}

		private void DoEyeAction()
		{
			inAction = true;
			string actionString = anims[Random.Range(0,anims.Length)];
			animator.SetTrigger(actionString);
			counting = false;
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

		private void PopulateAnimsArray()
		{
			anims[0] = "Close";
			anims[1] = "Look01";
			anims[2] = "Look01";
			anims[3] = "Look02";
			anims[4] = "Look02";
		}
	}
}
