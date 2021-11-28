using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.WorldMap
{
	public class LevelPinUIJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MMFeedbacks unCompJuice, CompJuice;

		public void PlayUnCompJuice()
		{
			unCompJuice.Initialization();
			unCompJuice.PlayFeedbacks();
		}

		public void PlayCompJuice()
		{
			CompJuice.Initialization();
			CompJuice.PlayFeedbacks();
		}
	}

}