using System.Collections;
using System.Collections.Generic;
using Qbism.PlayerCube;
using Qbism.SceneTransition;
using UnityEngine;
using UnityEngine.Events;

namespace Qbism.Cubes
{
	public class FinishCube : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip succesClip = null, failClip = null;

		//Cache
		PlayerCubeMover mover;
		CubeHandler handler;
		SceneHandler loader;
		AudioSource source;

		//States
		Vector2Int myPosition;

		public UnityEvent onFinishEvent = new UnityEvent();

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			handler = FindObjectOfType<CubeHandler>();
			loader = FindObjectOfType<SceneHandler>();
			source = GetComponentInChildren<AudioSource>();
		}

		private void OnEnable()
		{
			if (handler != null) handler.onLand += CheckForFinish;
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
					transform.forward), -1)) 
				{
					DestroyAllFloorCubes();
					StartCoroutine(NextLevelTransition());
				}		

				else StartCoroutine(RestartLevelTransition());
			}
		}

		private void DestroyAllFloorCubes()
		{
			foreach (KeyValuePair<Vector2Int, FloorCube> pair in handler.floorCubeDic)
			{
				var cube = pair.Value;
				if(cube.GetComponent<FinishCube>() || cube.hasShrunk == true) continue;

				cube.StartShrinking();
			}
		}

		private IEnumerator NextLevelTransition()
		{
			source.clip = succesClip;
			onFinishEvent.Invoke();
			yield return new WaitWhile(() => source.isPlaying);
			loader.NextLevel();
		}

		private IEnumerator RestartLevelTransition()
		{
			source.clip = failClip;
			onFinishEvent.Invoke();
			yield return new WaitWhile(() => source.isPlaying);
			loader.RestartLevel();
		}

		private void OnDisable()
		{
			if (handler != null) handler.onLand -= CheckForFinish;
		}
	}

}