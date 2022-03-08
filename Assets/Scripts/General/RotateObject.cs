using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class RotateObject : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Vector2 minMaxRotSpeed;
		[SerializeField] Vector3 rotateAxis;

		//States
		float rotSpeed;

		private void Start()
		{
			rotSpeed = Random.Range(minMaxRotSpeed.x, minMaxRotSpeed.y) * Time.deltaTime;
		}

		private void Update()
		{
			transform.Rotate(rotateAxis * rotSpeed, relativeTo: Space.Self);
		}
	}
}
