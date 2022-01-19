using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFloorChecker : MonoBehaviour
{
	//Config parameters
	[SerializeField] GameObject groundTile;
	[SerializeField] float sphereRadius = .2f;
	[SerializeField] LayerMask affectedLayers;

	private void Start()
	{
		Collider[] colls = Physics.OverlapSphere(transform.position, sphereRadius, affectedLayers,
			QueryTriggerInteraction.Collide);

		foreach (Collider coll in colls)
		{
			var oldWall = coll.GetComponent<TempOldWallID>();
			if (oldWall != null) groundTile.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
