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
		[SerializeField] MeshRenderer effectFace;
		[SerializeField] MoveableCube moveCube;
		[SerializeField] Transform moveableParent;
		public CubeTypes effectorType;

		private void Start()
		{
			UpdateFacePos();
		}

		public void ToggleEffectFace(bool value)
		{
			effectFace.enabled = value;
		}

		public void UpdateFacePos()
		{
			effectFace.transform.position = moveCube.transform.position;
		}

		public void AlignCubeRotToFaceRot()
		{
			if (effectorType == CubeTypes.Static) return;

			moveCube.transform.rotation = effectFace.transform.rotation;
		}

		public void AddMoveEffectorComponents(GameObject cube)
		{
			if (effectorType == CubeTypes.Boosting)
				GetComponent<BoostComponentAdder>().AddBoostComponent(cube);

			else if (effectorType == CubeTypes.Turning)
				GetComponent<TurnComponentAdder>().AddTurnComopnent(cube);


			else if (effectorType == CubeTypes.Static)
				GetComponent<StaticComponentAdder>().AddStaticComponent(cube);
		}

		public void ParentFaceToMoveable()
		{
			effectFace.transform.parent = moveCube.transform;
		}

		public void UnParentFace()
		{
			effectFace.transform.parent = moveableParent.transform;
		}
	}
}
