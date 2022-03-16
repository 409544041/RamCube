using MoreMountains.Feedbacks;
using Qbism.Cubes;
using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class TurnComponentAdder : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool isLeftTurning = false;
		[SerializeField] CubeRefHolder refs;

		public void AddTurnComopnent(GameObject cube)
		{
			var newTurn = cube.AddComponent<TurningCube>();
			newTurn.isLeftTurning = isLeftTurning;
			refs.turnCube = newTurn;
			newTurn.refs = refs;

			var newFlipper = cube.AddComponent<EditorFlipArrows>();
			refs.arrowFlip = newFlipper;
			newFlipper.refs = refs;
		}
	}
}
