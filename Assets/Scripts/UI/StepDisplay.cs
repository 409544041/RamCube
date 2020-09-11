using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using UnityEngine;
using UnityEngine.UI;

namespace Qbism.UI
{
	public class StepDisplay : MonoBehaviour
	{
		//Config paramters
		[SerializeField] int minSteps = 0;

		//Cache
		CubeHandler handler;
		Text stepText;

		//States
		int stepCounter = 0;

		private void Awake()
		{
			handler = FindObjectOfType<CubeHandler>();
			stepText = GetComponent<Text>();
		}

		private void OnEnable()
		{
			if (handler != null) handler.onLand += addToStepCounter;
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
			if (handler != null) handler.onLand -= addToStepCounter;
		}
	}
}
