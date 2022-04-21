using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Demo
{
	public class DemoScreenInputHandler : MonoBehaviour
	{
		//Cache
		GameControls controls;
		DemoScreenNavigator navigator;

		private void Awake()
		{
			controls = new GameControls();
			controls.Gameplay.XKey.performed += ctx => GoNext();

			navigator = GetComponent<DemoScreenNavigator>();
		}

		private void OnEnable()
		{
			controls.Gameplay.Enable();
		}

		private void GoNext()
		{
			navigator.GoNext();
		}

		private void OnDisable()
		{
			controls.Gameplay.Disable();
		}
	}
}
