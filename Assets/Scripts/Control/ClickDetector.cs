using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
	//Config parameters
	[SerializeField] LayerMask layerMask;

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 
				out hit, Mathf.Infinity, layerMask))
				hit.collider.GetComponent<ClickableObject>().ClickReaction();
		}
	}
}
