using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.Cubes;
using UnityEngine;

public class InterfacePulser : MonoBehaviour
{
	//Config parameters
	[SerializeField] InterfaceIDs interfaceID;
	[SerializeField] MMFeedbacks pulser = null;

	//Cache
	FinishCube finishCube = null;

	private void Awake() 
	{
		finishCube = FindObjectOfType<FinishCube>();
	}

	private void OnEnable() 
	{
		if(finishCube != null)
		{
			finishCube.onRewindPulse += InitiatePulse;
			finishCube.onStopRewindPulse += StopPulse;
		} 
	}

	private void InitiatePulse(InterfaceIDs id)
	{
		if (id == interfaceID)
		{
			pulser.Initialization();
			pulser.PlayFeedbacks();
		}
	}

	private void StopPulse(InterfaceIDs id)
	{
		pulser.StopFeedbacks();
	}

	private void OnDisable()
	{
		if (finishCube != null)
		{
			finishCube.onRewindPulse -= InitiatePulse;
			finishCube.onStopRewindPulse -= StopPulse;
		}
	}
}
