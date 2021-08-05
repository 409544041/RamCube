using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.General
{
	public class VisualsSwitch : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Renderer[] meshes;
		[SerializeField] SpriteRenderer[] sprites;

		//Cache
		PlayerFartLauncher farter;
		PlayerAnimator playerAnimator;

		private void Awake() 
		{
			farter = GetComponent<PlayerFartLauncher>();
			playerAnimator = GetComponentInChildren<PlayerAnimator>();
		}

		private void OnEnable() 
		{
			if (farter != null)
			{
				farter.onSwitchVisuals += SwitchMeshes;
				farter.onSwitchVisuals += SwitchSprites;
			}

			if (playerAnimator != null)
			{
				playerAnimator.onSwitchVisuals += SwitchMeshes;
				playerAnimator.onSwitchVisuals += SwitchSprites;
			}
		}

		private void SwitchMeshes(bool value)
		{
			foreach (var mesh in meshes)
			{
				mesh.enabled = value;
			}
		}

		private void SwitchSprites(bool value)
		{
			foreach (var sprite in sprites)
			{
				sprite.enabled = value;
			}
		}

		private void OnDisable()
		{
			if (farter != null)
			{
				farter.onSwitchVisuals -= SwitchMeshes;
				farter.onSwitchVisuals -= SwitchSprites;
			}

			if (playerAnimator != null)
			{
				playerAnimator.onSwitchVisuals -= SwitchMeshes;
				playerAnimator.onSwitchVisuals -= SwitchSprites;
			}
		}
	}
}
