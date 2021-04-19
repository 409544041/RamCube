using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Shapies
{
	public class ShapieHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Transform pushBackTarget;
		[SerializeField] MMFeedbacks pushBackJuice;

		public void pushBack()
		{
			pushBackJuice.Initialization();
			pushBackJuice.PlayFeedbacks();
		}
	}
}
