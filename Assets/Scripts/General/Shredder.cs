using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class Shredder : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			Destroy(other.gameObject);
		}
	}
}
