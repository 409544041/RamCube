using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class MusicOrderHandler : MonoBehaviour
	{
		//Config parameters
		public bool muteMusic = false;
		
		//States
		public int currentTrack { get; set; } = 0;
	}
}