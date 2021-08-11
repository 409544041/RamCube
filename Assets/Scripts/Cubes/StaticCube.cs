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
		[SerializeField] MMFeedbacks faceShrinkJuice;
		public GameObject face;

		public void BecomeShrinkingCube(GameObject cube)
		{
			faceShrinkJuice.PlayFeedbacks();
			GetComponent<FloorCube>().type = CubeTypes.Shrinking;
		}
	}
}

