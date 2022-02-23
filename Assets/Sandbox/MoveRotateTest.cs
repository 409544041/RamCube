using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotateTest : MonoBehaviour
{
	//Config parameters
	[SerializeField] float rollMultiplier = 2f;

	//States
	Vector3 prevPos;
	float currentSpeed;

	void Update()
    {
		CalculateCurrentSpeed();
		var rot = Quaternion.Euler(currentSpeed * rollMultiplier, 0, 0);
		transform.rotation *= rot;
	}

	private void CalculateCurrentSpeed()
	{
		var currentMovement = transform.position - prevPos;
		currentSpeed = currentMovement.magnitude / Time.deltaTime;
		prevPos = transform.position;
	}
}
