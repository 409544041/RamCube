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
	public bool input { get; private set;} = true;
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
	private void OnEnable() 
	{
		SwipeDetector.onSwipe += HandleSwipeInput;
		SwipeDetector.onTap += HandleTapInput;
	}

	private void Start() 
	{
		UpdatePositions();
	}

	void Update()
	{
		HandleKeyInput();
	}

	private void HandleSwipeInput(SwipeDetector.SwipeDirection direction)
	{
		if(!input) return;

		if(direction == SwipeDetector.SwipeDirection.up && onRaycast(Vector3.forward))
			StartCoroutine(Move(up, Vector3.right));

		if (direction == SwipeDetector.SwipeDirection.down && onRaycast(Vector3.back))
			StartCoroutine(Move(down, Vector3.left));

		if (direction == SwipeDetector.SwipeDirection.left && onRaycast(Vector3.left))
			StartCoroutine(Move(left, Vector3.forward));

		if (direction == SwipeDetector.SwipeDirection.right && onRaycast(Vector3.right))
			StartCoroutine(Move(right, Vector3.back));
	}

	private void HandleTapInput()
	{
		if(!input) return;
		
		if(isInBoostPos && !isBoosting && FireRamRaycast())
			StartCoroutine(Boost());
	}

	private void HandleKeyInput()
	{
		if (!input) return;

		if (Input.GetKeyDown(KeyCode.W) && onRaycast(Vector3.forward))
			StartCoroutine(Move(up, Vector3.right));

		if (Input.GetKeyDown(KeyCode.S) && onRaycast(Vector3.back))
			StartCoroutine(Move(down, Vector3.left));

		if (Input.GetKeyDown(KeyCode.A) && onRaycast(Vector3.left))
			StartCoroutine(Move(left, Vector3.forward));

		if (Input.GetKeyDown(KeyCode.D) && onRaycast(Vector3.right))
			StartCoroutine(Move(right, Vector3.back));

		if (Input.GetKeyDown(KeyCode.Space) && isInBoostPos && !isBoosting && FireRamRaycast())
			StartCoroutine(Boost());
	}

	private IEnumerator Move(Transform side, Vector3 turnAxis)
	{
		input = false;
		rb.isKinematic = true;

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

	private IEnumerator Boost()
	{
		input = false;
		rb.isKinematic = true;
		isBoosting = true;

		var tileToDrop = FetchCubeGridPos();

		while (isBoosting)
		{
			transform.position += transform.forward * boostSpeed * Time.deltaTime;
			yield return null;
		}

		transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
			0.5f, Mathf.RoundToInt(transform.position.z));

		UpdatePositions();

		dropper.DropTile(tileToDrop);

		isBoosting = false;
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
		if (Physics.BoxCast(transform.position, transform.localScale * .25f, transform.TransformDirection(Vector3.forward), out hit, transform.rotation, 1, 1 << 8))
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

	private void OnDisable() 
	{
		SwipeDetector.onSwipe -= HandleSwipeInput;
		SwipeDetector.onTap -= HandleTapInput;
	}
}
