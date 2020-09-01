using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTile : MonoBehaviour
{
	//Config parameters
	[SerializeField] Collider boostCollider;

	public void AttachBoostCollider(GameObject cube)
	{
		boostCollider.enabled = true;
		boostCollider.transform.parent = cube.transform;
	}

}
