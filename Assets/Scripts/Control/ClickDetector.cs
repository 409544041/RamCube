using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Qbism.Control
{
	public class ClickDetector : MonoBehaviour
	{
		//Config parameters
		[SerializeField] LayerMask layerMask;

		//Cache
		GameControls controls;

		private void Awake() 
		{
			controls = new GameControls();

			controls.Gameplay.Click.performed += ctx => HandleMouseClick();	
		}

		private void OnEnable() 
		{
			controls.Gameplay.Enable();
		}

		private void HandleMouseClick()
		{
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()),
				out hit, Mathf.Infinity, layerMask))
				hit.collider.GetComponent<ClickableObject>().ClickReaction();
		}

		private void OnDisable() 
		{
			controls.Gameplay.Disable();	
		}
	}
}

