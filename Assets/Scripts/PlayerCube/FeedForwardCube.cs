using System.Collections;
using System.Collections.Generic;
using Qbism.Core;
using Qbism.Cubes;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class FeedForwardCube : MonoBehaviour
	{
		//Cache
		CubeHandler handler;

		//States
		public bool isBoosting { get; set; } = false;

		private void Awake()
		{
			handler = FindObjectOfType<CubeHandler>();
		}

		public void CheckFloorInNewPos()
		{
			var currentCube = handler.FetchCube(FetchGridPos());

			if (currentCube.FetchType() == CubeTypes.Boosting)
				currentCube.GetComponent<BoostCube>().PrepareBoost(this.gameObject);

			else if (currentCube.FetchType() == CubeTypes.Flipping)
			{
				currentCube.GetComponent<FlipCube>().StartFlip(this.gameObject);
			}
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
