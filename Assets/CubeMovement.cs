using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
	//Config parameters
	[SerializeField] Transform center;
	[SerializeField] Transform up;
	[SerializeField] Transform down;
	[SerializeField] Transform left;
	[SerializeField] Transform right;
	[SerializeField] int turnStep = 9;
	[SerializeField] Transform thruster;
	[SerializeField] float boostSpeed = .3f;

	//Cache
	Rigidbody rb;

	//States
	public bool canBoost {get; set;} = true;
	bool input = true;
	public bool isBoosting {get; set;} = false;

	private void Awake() 
	{
		rb = GetComponent<Rigidbody>();	
	}

	void Update()
	{
		if(input == true)
		{
			if(Input.GetKeyDown(KeyCode.W))
			{
				StartCoroutine(Move(up, Vector3.right));
				input = false;
			}
			if (Input.GetKeyDown(KeyCode.S))
			{
				StartCoroutine(Move(down, Vector3.left));
				input = false;
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				StartCoroutine(Move(left, Vector3.forward));
				input = false;
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				StartCoroutine(Move(right, Vector3.back));
				input = false;
			}
			if(Input.GetKeyDown(KeyCode.Space) && canBoost)
			{
				input = false;
				canBoost = false;
				isBoosting = true;
				StartCoroutine(Boost());
			}
		}
	}

	private IEnumerator Boost()
	{
		//transform.Translate(Vector3.forward);

		while(isBoosting)
		{
			//rb.velocity = Vector3.forward * boostSpeed;
			transform.position += transform.forward * boostSpeed * Time.deltaTime;
			yield return null;
		}

		transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
		UpdatePositions();
		input = true;			
	}

	private IEnumerator Move(Transform side, Vector3 turnAxis)
	{
		for (int i = 0; i < (90 / turnStep); i++)
		{
			transform.RotateAround(side.position, turnAxis, turnStep);
			yield return null;
		}
		UpdatePositions();
		input = true;
	}

	private void UpdatePositions()
	{
		center.position = transform.position;
	}	
}
