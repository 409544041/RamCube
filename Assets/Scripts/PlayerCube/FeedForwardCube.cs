using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class FeedForwardCube : MonoBehaviour
	{

		//States
		public bool isBoosting { get; set; } = false;

		public event Action<Vector2Int, GameObject> onFeedForwardFloorCheck;

		private void Start() 
		{
			this.gameObject.SetActive(false);	
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
				0.5f, Mathf.RoundToInt(transform.position.z));

			Quaternion rotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.x),
				Mathf.RoundToInt(transform.rotation.y), Mathf.RoundToInt(transform.rotation.z));
		}

		private void OnDisable()
		{
			isBoosting = false;
		}
	}
}
