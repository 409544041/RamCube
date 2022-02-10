using Qbism.Cubes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class BoostComponentAdder : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Transform boostRayOrigin;
		[SerializeField] GameObject objDir;
		[SerializeField] LayerMask boostMaskPlayer;
		[SerializeField] LayerMask boostMaskMoveable;

		public void AddBoostComponent(GameObject cube)
		{
			BoostCube newBoost = cube.AddComponent<BoostCube>();
			newBoost.boostRayOrigin = boostRayOrigin;
			newBoost.boostObjDir = objDir;
			newBoost.boostMaskPlayer = boostMaskPlayer;
			newBoost.boostMaskMoveable = boostMaskMoveable;
		}
	}
}
