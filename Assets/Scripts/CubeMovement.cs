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
	TileHandler handler;

	//States
	public bool isInBoostPos { get; set; } = true;
	public bool input { get; private set;} = true;
	public bool isBoosting { get; set; } = false;
	public bool canMoveUp { get; set; } = true;
	public bool canMoveDown { get; set; } = true;
	public bool canMoveLeft { get; set; } = true;
	public bool canMoveRight { get; set; } = true;

	Vector2Int tileAbovePos = new Vector2Int(0, 1);
	Vector2Int tileBelowPos = new Vector2Int(0, -1);
	Vector2Int tileLeftPos = new Vector2Int(-1, 0);
	Vector2Int tileRightPos = new Vector2Int(1, 0);

	public delegate bool RayCastDelegate(Vector3 direction);
	public RayCastDelegate onRaycast;


	private void Awake() 
	{
		rb = GetComponent<Rigidbody>();	
		handler = FindObjectOfType<TileHandler>();
	}
	private void OnEnable() 
	{
		SwipeDetector.onSwipe += HandleSwipeInput;
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

		if(direction == SwipeDetector.SwipeDirection.up && 
			handler.FetchTile(FetchCubeGridPos() + tileAbovePos).GetComponent<FloorTile>())
			StartCoroutine(Move(up, Vector3.right));

		if (direction == SwipeDetector.SwipeDirection.down && 
			handler.FetchTile(FetchCubeGridPos() + tileBelowPos).GetComponent<FloorTile>())
			StartCoroutine(Move(down, Vector3.left));

		if (direction == SwipeDetector.SwipeDirection.left && 
			handler.FetchTile(FetchCubeGridPos() + tileLeftPos).GetComponent<FloorTile>())
			StartCoroutine(Move(left, Vector3.forward));

		if (direction == SwipeDetector.SwipeDirection.right && 
			handler.FetchTile(FetchCubeGridPos() + tileRightPos).GetComponent<FloorTile>())
			StartCoroutine(Move(right, Vector3.back));
	}

	private void HandleKeyInput()
	{
		if (!input) return;

		if (Input.GetKeyDown(KeyCode.W) && 
			handler.FetchTile(FetchCubeGridPos() + tileAbovePos))
			StartCoroutine(Move(up, Vector3.right));

		if (Input.GetKeyDown(KeyCode.S) && 
			handler.FetchTile(FetchCubeGridPos() + tileBelowPos))
			StartCoroutine(Move(down, Vector3.left));

		if (Input.GetKeyDown(KeyCode.A) && 
			handler.FetchTile(FetchCubeGridPos() + tileLeftPos))
			StartCoroutine(Move(left, Vector3.forward));

		if (Input.GetKeyDown(KeyCode.D) && 
			handler.FetchTile(FetchCubeGridPos() + tileRightPos))
			StartCoroutine(Move(right, Vector3.back));
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Vector2Int currentPos = FetchCubeGridPos();
			Vector2Int tileAbovePos = new Vector2Int(currentPos.x, currentPos.y + 1);
			print(tileAbovePos);
			if(handler.FetchTile(tileAbovePos))
			print("can move");
		}
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

		RoundPosition();
		UpdatePositions();

		handler.DropTile(tileToDrop);

		rb.isKinematic = false;

		CheckFloorInNewPos();
	}

	private IEnumerator Boost(Vector3 boostDirection)
	{
		input = false;
		rb.isKinematic = true;
		isBoosting = true;

		var tileToDrop = FetchCubeGridPos();

		while (isBoosting)
		{
			transform.position += boostDirection * boostSpeed * Time.deltaTime;
			yield return null;
		}
		
		RoundPosition();
		UpdatePositions();

		handler.DropTile(tileToDrop);

		isBoosting = false;
		rb.isKinematic = false;

		CheckFloorInNewPos();
	}

	private void RoundPosition()
	{
		transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
			0.5f, Mathf.RoundToInt(transform.position.z));
		
		Quaternion rotation = Quaternion.Euler(Mathf.RoundToInt(transform.rotation.x), 
			Mathf.RoundToInt(transform.rotation.y), Mathf.RoundToInt(transform.rotation.z));
	}

	private void CheckFloorInNewPos()
	{
		FloorTile currentTile = handler.FetchTile(FetchCubeGridPos());

		if (!currentTile.hasFallen)
		{
			input = true;
		}

		if(currentTile.FetchType() == TileTypes.Boosting)
		{
			currentTile.GetComponent<BoostTile>().AttachBoostCollider(this.gameObject);
			StartCoroutine(Boost(handler.FetchTile(FetchCubeGridPos()).transform.forward));
		}
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
	}
}
