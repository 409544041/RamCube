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
		FeedForwardCube[] ffCubes;

		private void Awake() 
		{
			farter = GetComponent<PlayerFartLauncher>();
			playerAnimator = GetComponentInChildren<PlayerAnimator>();
			ffCubes = GetComponentsInParent<FeedForwardCube>();
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

			if (ffCubes != null)
			{
				foreach (var cube in ffCubes)
				{
					cube.onSwitchVisuals += SwitchMeshes;
				}
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
