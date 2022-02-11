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
		[SerializeField] MMFeedbacks faceShrinkJuice;
		[SerializeField] GameObject face;
		[SerializeField] GameObject shrinkingFace;

		public void AddStaticComponent(GameObject cube)
		{
			var newStatic = cube.AddComponent<StaticCube>();
			newStatic.faceShrinkJuice = faceShrinkJuice;
			newStatic.face = face;
			newStatic.shrinkingFace = shrinkingFace;
		}
	}
}
