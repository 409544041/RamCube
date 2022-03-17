using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.General;
using Qbism.MoveableCubes;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class FeedForwardCube : MonoBehaviour
	{
		//Config paremeters
		public CubePositioner cubePoser;
		[SerializeField] FFJuicer ffJuicer;
		[SerializeField] VisualsSwitch visualSwitch;

		//States
		public bool isBoosting { get; set; } = false;
		public bool isOutOfBounds { get; set; } = false;

		//Actions, events, delegates etc
		public event Action<Vector2Int, GameObject> onFeedForwardFloorCheck;

		public void CheckFloorInNewPos()
		{
			onFeedForwardFloorCheck(cubePoser.FetchGridPos(), this.gameObject);
		}

		public void SwitchFF(bool value)
		{
			if (value == true)
			{
				ffJuicer.TriggerJuice();
				visualSwitch.SwitchMeshes(true);
			}
			else visualSwitch.SwitchMeshes(false);
		}

		private void OnDisable()
		{
			isBoosting = false;
			isOutOfBounds = false;
		}
	}
}
