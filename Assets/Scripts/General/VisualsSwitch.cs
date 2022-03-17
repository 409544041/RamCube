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
		[SerializeField] PlayerRefHolder refs;

		public void SwitchMeshes(bool value)
		{
			foreach (var mesh in meshes)
			{
				mesh.enabled = value;
			}
		}

		public void SwitchSprites(bool value)
		{
			foreach (var sprite in sprites)
			{
				sprite.enabled = value;
			}
		}
	}
}
