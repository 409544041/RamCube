using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
	//Config parameters
	[SerializeField] bool isStatic = false;
	[SerializeField] Material staticMaterial = null;

	//States
	public bool hasFallen { get; set; } = false;

	private void Start() 
	{
		if(isStatic) GetComponent<MeshRenderer>().material = staticMaterial;
	}

	public Vector2Int FetchTileGridPos()
	{
		return new Vector2Int
			(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
	}

	public bool FetchIsStatic()
	{
		return isStatic;
	}
}
