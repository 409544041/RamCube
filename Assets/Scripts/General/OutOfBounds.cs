using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.General
{
	public class OutOfBounds : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other) 
		{
			if(other.tag == "Player")
				FindObjectOfType<SceneHandler>().RestartLevel();
		}
	}
}
