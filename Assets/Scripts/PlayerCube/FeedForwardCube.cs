using System;
using System.Collections;
using System.Collections.Generic;
using Qbism.MoveableCubes;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class FeedForwardCube : MonoBehaviour, IActiveCube
	{
		//Cache
		FFJuicer ffJuicer;

		//States
		public bool isBoosting { get; set; } = false;
		public bool isOutOfBounds { get; set; } = false;

		//Actions, events, delegates etc
		public event Action<Vector2Int, GameObject> onFeedForwardFloorCheck;
		public event Action<bool> onSwitchVisuals;

		private void Awake() 
		{
			ffJuicer = GetComponent<FFJuicer>();
		}

		public void CheckFloorInNewPos()
		{
			onFeedForwardFloorCheck(FetchGridPos(), this.gameObject);
		}

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}

		public void RoundPosition()
		{
			transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
				Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));

			Quaternion rotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.x),
				Mathf.RoundToInt(transform.rotation.y), Mathf.RoundToInt(transform.rotation.z));
		}

		public void SwitchFF(bool value)
		{
			if (value == true)
			{
				ffJuicer.TriggerJuice();
				onSwitchVisuals(true);
			}
			else onSwitchVisuals(false);
		}

		private void OnDisable()
		{
			isBoosting = false;
			isOutOfBounds = false;
		}
	}
}
