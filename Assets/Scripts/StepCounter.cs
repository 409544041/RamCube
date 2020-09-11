using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepCounter : MonoBehaviour
{
	//Config paramters
	[SerializeField] Text stepText = null;
	[SerializeField] int minSteps = 0;

	//Cache
	PlayerCubeMover mover;

	//States
	int stepCounter = 0;

	private void Awake() 
	{
		mover = FindObjectOfType<PlayerCubeMover>();
	}

	private void OnEnable() 
	{
		if(mover != null) mover.onLand += addToStepCounter;
	}

	private void Update() 
	{
		stepText.text = stepCounter +  " / " + minSteps;
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
