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
	TileDropper dropper;

	//States
	public bool isInBoostPos { get; set; } = true;
	bool input = true;
	public bool isBoosting { get; set; } = false;
	public bool canMoveUp { get; set; } = true;
	public bool canMoveDown { get; set; } = true;
	public bool canMoveLeft { get; set; } = true;
	public bool canMoveRight { get; set; } = true;

	public delegate bool RayCastDelegate(Vector3 direction);
	public RayCastDelegate onRaycast;


	private void Awake() 
	{
		rb = GetComponent<Rigidbody>();	
		dropper = FindObjectOfType<TileDropper>();
	}

	private void Start() 
	{
		UpdatePositions();
	}

	void Update()
	{
		if(input == true)
		{
			if(Input.GetKeyDown(KeyCode.W) && onRaycast(Vector3.forward))
			{
				StartCoroutine(Move(up, Vector3.right));
				input = false;
				rb.isKinematic = true;
			}
			if (Input.GetKeyDown(KeyCode.S) && onRaycast(Vector3.back))
			{
				StartCoroutine(Move(down, Vector3.left));
				input = false;
				rb.isKinematic = true;
			}
			if (Input.GetKeyDown(KeyCode.A) && onRaycast(Vector3.left))
			{
				StartCoroutine(Move(left, Vector3.forward));
				input = false;
				rb.isKinematic = true;
			}
			if (Input.GetKeyDown(KeyCode.D) && onRaycast(Vector3.right))
			{
				StartCoroutine(Move(right, Vector3.back));
				input = false;
				rb.isKinematic = true;
			}
			if(Input.GetKeyDown(KeyCode.Space) && isInBoostPos && !isBoosting && FireRamRaycast())
			{
				input = false;
				rb.isKinematic = true;
				isBoosting = true;
				StartCoroutine(Boost());
			}
		}
	}

	private IEnumerator Boost()
	{
		while(isBoosting)
		{
			transform.position += transform.forward * boostSpeed * Time.deltaTime;
			yield return null;
		}

		transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), 
			0.5f, Mathf.RoundToInt(transform.position.z));

		UpdatePositions();

		isBoosting = false;
		rb.isKinematic = false;

		CheckFloorInNewPos();			
	}

	private IEnumerator Move(Transform side, Vector3 turnAxis)
	{
		var tileToDrop = FetchCubeGridPos();

		for (int i = 0; i < (90 / turnStep); i++)
		{
			transform.RotateAround(side.position, turnAxis, turnStep);
			yield return null;
		}

		UpdatePositions();

		dropper.DropTile(tileToDrop);

		rb.isKinematic = false;

		CheckFloorInNewPos();
	}

	private void CheckFloorInNewPos()
	{
		if (!dropper.FetchTileFallen(FetchCubeGridPos()))
		{
			input = true;
		}
	}

	public bool FireRamRaycast()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1, 1 << 8))
			return false;
		else return true;
	}

	private void UpdatePositions()
	{
		center.position = transform.position;
	}

	public Vector2Int FetchCubeGridPos()
	{
		return new Vector2Int
			(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
	}
}
