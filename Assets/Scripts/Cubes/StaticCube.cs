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

		public void BecomeShrinkingCube(GameObject cube)
		{
			refs.staticFaceShrinkJuice.Initialization();
			refs.staticFaceShrinkJuice.PlayFeedbacks();
			refs.floorCube.type = CubeTypes.Shrinking;
		}

		public void SwitchFaces()
		{
			refs.staticFace.SetActive(false);
			refs.staticShrinkingFace.SetActive(true);
		}
	}
}

