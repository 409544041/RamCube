using Qbism.Cubes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class BoostComponentAdder : MonoBehaviour
	{
		//Config parameters
		[SerializeField] LayerMask boostMaskPlayer;
		[SerializeField] LayerMask boostMaskMoveable;
		[SerializeField] CubeRefHolder refs;

		public void AddBoostComponent(GameObject cube)
		{
			BoostCube newBoost = cube.AddComponent<BoostCube>();
			newBoost.boostMaskPlayer = boostMaskPlayer;
			newBoost.boostMaskMoveable = boostMaskMoveable;
			refs.boostCube = newBoost;
			newBoost.refs = refs;
		}
	}
}
