using System.Collections;
using System.Collections.Generic;
using Qbism.Core;
using Qbism.PlayerCube;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FinishCube : MonoBehaviour
	{
		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		SceneHandler loader;

		//States
		Vector2Int myPosition;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = FindObjectOfType<CubeHandler>();
			loader = FindObjectOfType<SceneHandler>();
		}

		private void OnEnable()
		{
			if (mover != null) mover.onLand += CheckForFinish;
		}

		private void Start()
		{
			myPosition = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
		}

		private void CheckForFinish()
		{
			if (handler.FetchCube(myPosition) == handler.FetchCube(mover.FetchGridPos()))
			{
				if (Mathf.Approximately(Vector3.Dot(mover.transform.forward,
					transform.forward), -1)) loader.NextLevel();
				else loader.RestartLevel();
			}
		}

		private void OnDisable()
		{
			if (mover != null) mover.onLand -= CheckForFinish;
		}
	}

}