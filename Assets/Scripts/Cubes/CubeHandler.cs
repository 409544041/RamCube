using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeHandler : MonoBehaviour
	{
		//Cache
		FeedForwardCube[] ffCubes = null;
		PlayerCubeFeedForward cubeFF = null;
		PlayerCubeMover mover = null;
		MoveableCube[] moveableCubes = null;
		MoveableCubeHandler moveHandler;
		PlayerCubeFlipJuicer playerFlipJuicer;
		PlayerCubeBoostJuicer playerBoostJuicer;

		//States
		public FloorCube currentCube { get; set; } = null;
		
		public Dictionary<Vector2Int, FloorCube> floorCubeDic = 
			new Dictionary<Vector2Int, FloorCube>();
		
		public event Action onLand;
		public event Action<FloorCube, Vector3, Quaternion, Vector3> onInitialCubeRecording;

		private void Awake()
		{
			mover = FindObjectOfType<PlayerCubeMover>();
			ffCubes = FindObjectsOfType<FeedForwardCube>();
			cubeFF = FindObjectOfType<PlayerCubeFeedForward>();
			moveableCubes = FindObjectsOfType<MoveableCube>();
			moveHandler = GetComponent<MoveableCubeHandler>();
			playerFlipJuicer = mover.GetComponent<PlayerCubeFlipJuicer>();
			playerBoostJuicer = mover.GetComponent<PlayerCubeBoostJuicer>();

			LoadFloorCubeDictionary();
		}

		private void OnEnable() 
		{
			if (mover != null)
			{
				mover.onFloorCheck += CheckFloorType;
				mover.onCubeShrink += ShrinkCube;
				mover.onInitialFloorCubeRecord += InitialRecordCubes;
			} 

			if (cubeFF != null)
			{
				cubeFF.onKeyCheck += CheckFloorCubeDicKey;
				cubeFF.onShrunkCheck += FetchShrunkStatus;
			} 

			if (ffCubes != null)
			{
				foreach (FeedForwardCube ffCube in ffCubes)
				{
					ffCube.onFeedForwardFloorCheck += CheckFloorTypeForFF;
				}
			}

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck += CheckFloorCubeDicKey;
					cube.onComponentAdd += AddComponent;
					cube.onFloorCheck += CheckFloorTypeForMoveable;
					cube.onShrunkCheck += FetchShrunkStatus;
					cube.onSetFindable += SetFindableStatus;
					cube.onDicRemove += RemoveFromDictionary;
					cube.onSetShrunk += SetShrunkStatus;
				}
			}
		}

		private void Start() 
		{
			currentCube = FetchCube(mover.FetchGridPos());
		}

		public void LoadFloorCubeDictionary()
		{
			FloorCube[] cubes = FindObjectsOfType<FloorCube>();
			foreach (FloorCube cube in cubes)
			{
				Vector2Int pos = cube.FetchGridPos();
				if (floorCubeDic.ContainsKey(pos))
					print("Overlapping cube " + cube + " & " + floorCubeDic[pos]);
				else floorCubeDic.Add(cube.FetchGridPos(), cube);
			}
		}

		public void ShrinkCube(Vector2Int cubeToShrink)
		{
			if (floorCubeDic[cubeToShrink].FetchType() == CubeTypes.Shrinking)
			{
				floorCubeDic[cubeToShrink].GetComponent<CubeShrinker>().StartShrinking();
			}
		}

		private void CheckFloorType(Vector2Int cubePos, GameObject cube, 
			Transform side, Vector3 turnAxis, Vector2Int posAhead)
		{
			FloorCube previousCube;

			previousCube = currentCube;

			if(previousCube.FetchType() == CubeTypes.Static)
				previousCube.GetComponent<StaticCube>().BecomeShrinkingCube(cube);
			
			if(previousCube.FetchType() == CubeTypes.Boosting && 
				moveHandler.CheckMoveableCubeDicKey(posAhead))
			{
				moveHandler.ActivateMoveableCube(posAhead, turnAxis, cubePos);
			}

			if (floorCubeDic.ContainsKey(cubePos))
			{
				currentCube = FetchCube(cubePos);
				bool differentCubes = currentCube != previousCube;

				if (currentCube.FetchType() == CubeTypes.Boosting)
					currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);

				else if ((currentCube.FetchType() == CubeTypes.Turning) && differentCubes)
				{
					if (onLand != null) onLand();
					currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
				}

				else
				{
					if (differentCubes && onLand != null)
					{
						if (!mover.isStunned) cubeFF.ShowFeedForward();
						onLand();

						if(previousCube.FetchType() != CubeTypes.Boosting)
						{
							playerFlipJuicer.PlayLandClip();
							playerFlipJuicer.PlayPostFlipJuice();
						}
						else
						{
							playerBoostJuicer.PlayPostBoostJuice();
						}
						
					}
					else
					{
						//landing on same cube, like after having turned/flipped
						if (!mover.isStunned) cubeFF.ShowFeedForward(); 
					}

					mover.GetComponent<PlayerFartLauncher>().ResetFartCollided();
				}
			}
			else
			{
				mover.InitiateLowering(cubePos);
			}
		}

		private void CheckFloorTypeForFF(Vector2Int cubePos, GameObject cube)
		{
			FloorCube currentCube = FetchCube(cubePos);

			if(currentCube.FetchType() == CubeTypes.Boosting ||
				currentCube.FetchType() == CubeTypes.Turning)
				currentCube.GetComponent<ICubeInfluencer>().PrepareAction(cube);
		}

		private void CheckFloorTypeForMoveable(Transform side, Vector3 turnAxis, Vector2Int posAhead,
			MoveableCube cube, Vector2Int cubePos, Vector2Int originPos, Vector2Int prevPos)
		{
			if(floorCubeDic.ContainsKey(cubePos))
			{
				FloorCube currentCube = FetchCube(cubePos);
				FloorCube prevCube = FetchCube(prevPos);

				if (currentCube.FetchType() == CubeTypes.Boosting ||
					(currentCube.FetchType() == CubeTypes.Turning))
				{
					if (prevCube.type == CubeTypes.Boosting &&
						moveHandler.CheckMoveableCubeDicKey(posAhead))
						moveHandler.ActivateMoveableCube(posAhead, turnAxis, cubePos);

					currentCube.GetComponent<ICubeInfluencer>().
					PrepareActionForMoveable(side, turnAxis, posAhead, cube.gameObject, originPos, prevCube);
				}
					
				else if(currentCube.FetchType() == CubeTypes.Shrinking ||
					currentCube.FetchType() == CubeTypes.Static)
				{
					if(prevCube.type == CubeTypes.Boosting && 
						moveHandler.CheckMoveableCubeDicKey(posAhead))
					{
						moveHandler.ActivateMoveableCube(posAhead, turnAxis, cubePos);
						cube.hasBumped = true;
					}
						
					cube.InitiateMove(side, turnAxis, posAhead, originPos);
				}
			}

			else cube.InitiateLowering(cubePos);
		}

		private void InitialRecordCubes()
		{
			foreach (KeyValuePair<Vector2Int, FloorCube> pair in floorCubeDic)
			{
				var cube = pair.Value;
				CubeShrinker shrinker = cube.GetComponent<CubeShrinker>();
				
				onInitialCubeRecording(cube, cube.transform.position, 
					cube.transform.rotation, cube.transform.localScale);
			}
		}

		public bool CheckFloorCubeDicKey(Vector2Int cubePos)
		{
			if(floorCubeDic.ContainsKey(cubePos)) return true;
			else return false;
		}

		private void AddComponent(Vector2Int cubePos, GameObject cube, float shrinkStep, 
			float shrinkTimeStep, MMFeedbacks shrinkFeedback, float shrinkDuration)
		{
			FloorCube newFloor = cube.AddComponent<FloorCube>();
			CubeShrinker newShrinker = cube.AddComponent<CubeShrinker>();
			newShrinker.shrinkStep = shrinkStep;
			newShrinker.timeStep = shrinkTimeStep;
			newFloor.tag = "Environment";
			newFloor.type = CubeTypes.Shrinking;
			newShrinker.shrinkFeedback = shrinkFeedback;
			newShrinker.shrinkFeedbackDuration = shrinkDuration;

			AddToDictionary(cubePos, newFloor);
		}

		public void AddToDictionary(Vector2Int cubePos, FloorCube cube)
		{
			floorCubeDic.Add(cubePos, cube);
		}

		private void RemoveFromDictionary(Vector2Int cubePos)
		{
			floorCubeDic.Remove(cubePos);
		}

		public FloorCube FetchCube(Vector2Int cubePos)
		{
			return floorCubeDic[cubePos];
		}

		public bool FetchShrunkStatus(Vector2Int cubePos)
		{
			FloorCube cube = FetchCube(cubePos);
			CubeShrinker shrinker = cube.GetComponent<CubeShrinker>();

			if (shrinker && shrinker.hasShrunk) return true;
			else return false;
		}

		private void SetFindableStatus(Vector2Int cubePos, bool value)
		{
			FetchCube(cubePos).isFindable = value;
		}

		private void SetShrunkStatus(Vector2Int cubePos, bool value)
		{
			if(FetchCube(cubePos).type == CubeTypes.Shrinking)
				FetchCube(cubePos).GetComponent<CubeShrinker>().hasShrunk = value;
		}

		private void OnDisable()
		{
			if (mover != null)
			{
				mover.onFloorCheck -= CheckFloorType;
				mover.onCubeShrink -= ShrinkCube;
				mover.onInitialFloorCubeRecord -= InitialRecordCubes;
			}

			if (cubeFF != null)
			{
				cubeFF.onKeyCheck -= CheckFloorCubeDicKey;
				cubeFF.onShrunkCheck -= FetchShrunkStatus;
			}

			if (ffCubes != null)
			{
				foreach (FeedForwardCube ffCube in ffCubes)
				{
					ffCube.onFeedForwardFloorCheck -= CheckFloorTypeForFF;
				}
			}

			if (moveableCubes != null)
			{
				foreach (MoveableCube cube in moveableCubes)
				{
					cube.onFloorKeyCheck -= CheckFloorCubeDicKey;
					cube.onComponentAdd -= AddComponent;
					cube.onFloorCheck -= CheckFloorTypeForMoveable;
					cube.onShrunkCheck -= FetchShrunkStatus;
					cube.onSetFindable -= SetFindableStatus;
					cube.onDicRemove -= RemoveFromDictionary;
					cube.onSetShrunk -= SetShrunkStatus;
				}
			}
		}
	}
}
