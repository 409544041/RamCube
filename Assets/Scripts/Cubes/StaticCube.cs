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
		public MMFeedbacks faceShrinkJuice;
		public GameObject face, shrinkingFace;

		public void BecomeShrinkingCube(GameObject cube)
		{
			faceShrinkJuice.Initialization();
			faceShrinkJuice.PlayFeedbacks();
			GetComponent<FloorCube>().type = CubeTypes.Shrinking;
		}

		public void SwitchFaces()
		{
			face.SetActive(false);
			shrinkingFace.SetActive(true);
		}
	}
}

