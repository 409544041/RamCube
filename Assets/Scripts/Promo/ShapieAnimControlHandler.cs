using System.Collections;
using System.Collections.Generic;
using Qbism.Shapies;
using UnityEngine;

namespace Qbism.Promo
{
	public class ShapieAnimControlHandler : MonoBehaviour
	{
		//Cache
		ShapieAnimForcer[] shapies;
		GameControls controls;

		private void Awake()
		{
			controls = new GameControls();
			controls.Gameplay.Debugkey3.performed += ctx => ForceDancing();
			controls.Gameplay.DebugKey4.performed += ctx => ForceLookAround();

			shapies = FindObjectsOfType<ShapieAnimForcer>();
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
}