using System.Collections;
using System.Collections.Generic;
using Qbism.Shapies;
using UnityEngine;

public class ForceDance : MonoBehaviour
{
	//Cache
	ShapieAnimator[] shapies;
	GameControls controls;

	private void Awake() 
	{
		controls = new GameControls();
		controls.Gameplay.DebugShapieDance.performed += ctx => ForceDancing();
		controls.Gameplay.DebugShapieLookaround.performed += ctx => ForceLookAround();

		shapies = FindObjectsOfType<ShapieAnimator>();
	}

	private void OnEnable()
	{
		controls.Gameplay.Enable();
	}

	private void ForceDancing()
	{
		foreach (var shapie in shapies)
		{
			shapie.ForceCelebrate();
		}
	}

	private void ForceLookAround()
	{
		foreach (var shapie in shapies)
		{
			shapie.ForceLookingAround();
		}
	}

	private void OnDisable()
	{
		controls.Gameplay.Disable();
	}
}
