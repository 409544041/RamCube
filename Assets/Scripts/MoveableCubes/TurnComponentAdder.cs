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
		[SerializeField] CubePositioner cubePoser;
		[SerializeField] MMFeedbacks turnJuicer;

		public void AddTurnComopnent(GameObject cube)
		{
			var newTurn = cube.AddComponent<TurningCube>();
			newTurn.isLeftTurning = isLeftTurning;
			newTurn.cubePoser = cubePoser;
			newTurn.juicer = turnJuicer;

			cube.AddComponent<EditorFlipArrows>();
		}
	}
}
