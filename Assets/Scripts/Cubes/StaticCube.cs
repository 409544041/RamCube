using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Cubes
{
	public class StaticCube : MonoBehaviour
	{
		//Config parameters
		public CubeRefHolder refs;

		public void BecomeShrinkingCube()
		{
			refs.staticFaceShrinkJuice.Initialization();
			refs.staticFaceShrinkJuice.PlayFeedbacks();
			refs.floorCube.type = CubeTypes.Shrinking;
		}
	}
}

