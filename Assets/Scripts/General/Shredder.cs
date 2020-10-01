using System.Collections;
using System.Collections.Generic;
using Qbism.SceneTransition;
using UnityEngine;

namespace Qbism.General
{
	public class Shredder : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Player") 
				FindObjectOfType<SceneHandler>().RestartLevel();

			else if (other.tag == "Environment")
			{
				other.GetComponent<MeshRenderer>().enabled = false;
				other.GetComponent<BoxCollider>().enabled = false;
				other.GetComponent<Rigidbody>().isKinematic = true;
			}

		}
	}
}
