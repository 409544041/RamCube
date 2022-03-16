using MoreMountains.Feedbacks;
using Qbism.Cubes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class StaticComponentAdder : MonoBehaviour
	{
		//Config parameters
		[SerializeField] CubeRefHolder refs;

		public void AddStaticComponent(GameObject cube)
		{
			var newStatic = cube.AddComponent<StaticCube>();
			refs.staticCube = newStatic;
			newStatic.refs = refs;
		}
	}
}
