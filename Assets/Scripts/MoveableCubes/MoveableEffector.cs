using Qbism.Cubes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class MoveableEffector : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Transform moveableParent;
		public CubeTypes effectorType;
		[SerializeField] CubeRefHolder refs;

		private void Start()
		{
			UpdateFacePos();
		}

		public void ToggleEffectFace(bool value)
		{
			refs.movFaceMesh.enabled = value;
		}

		public void UpdateFacePos()
		{
			refs.movFaceMesh.transform.position = refs.movCube.transform.position;
		}

		public void AlignCubeRotToFaceRot()
		{
			if (effectorType == CubeTypes.Static) return;

			refs.movCube.transform.rotation = refs.movFaceMesh.transform.rotation;
		}

		public void AddMoveEffectorComponents(GameObject cube)
		{
			if (effectorType == CubeTypes.Boosting)
				refs.boostCompAdder.AddBoostComponent(cube);

			else if (effectorType == CubeTypes.Turning)
				refs.turnCompAdder.AddTurnComopnent(cube);


			else if (effectorType == CubeTypes.Static)
				refs.staticCompAdder.AddStaticComponent(cube);
		}

		public void ParentFaceToMoveable()
		{
			refs.movFaceMesh.transform.parent = refs.movCube.transform;
		}

		public void UnParentFace()
		{
			refs.movFaceMesh.transform.parent = moveableParent.transform;
		}
	}
}
