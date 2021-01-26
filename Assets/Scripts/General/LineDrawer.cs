using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
	//Config parameters
	[SerializeField] float drawSpeed = 5f;

	//Cache
	LineRenderer lineRenderer;

	//States
	public bool drawing { get; set; } = false;
	float counter = 0;
	float distance = 0;
	public Transform origin { get; set; }
	public Transform destination { get; set; }

	private void Awake() 
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		AnimateLineDrawing();
	}

	private void AnimateLineDrawing()
	{
		if(!drawing) return;

		if (counter < distance)
		{
			counter += .1f / drawSpeed;
			float x = Mathf.Lerp(0, distance, counter);

			Vector3 pointAlongLine = x *
				Vector3.Normalize(destination.position - origin.position)
				+ origin.position;

			lineRenderer.SetPosition(1, pointAlongLine);
		}
	}

	public void SetPositions(Transform incOrigin, Transform incDestination)
	{
		origin = incOrigin;
		destination = incDestination;

		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, origin.position);
		distance = Vector3.Distance(origin.position, destination.position);
	}
}
