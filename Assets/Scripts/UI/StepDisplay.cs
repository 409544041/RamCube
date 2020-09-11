using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.UI
{
	public class StepDisplay : MonoBehaviour
	{
		//Config paramters
		[SerializeField] int minSteps = 0;

		//Cache
		PlayerCubeMover mover;
		Text stepText;

		//States
		int stepCounter = 0;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			stepText = GetComponent<Text>();
		}

		private void OnEnable()
		{
			if (mover != null) mover.onLand += addToStepCounter;
		}

		private void Update()
		{
			stepText.text = stepCounter + " / " + minSteps;
		}

		private void addToStepCounter()
		{
			stepCounter++;
		}

		private void OnDisable()
		{
			if (mover != null) mover.onLand -= addToStepCounter;
		}
	}
}
