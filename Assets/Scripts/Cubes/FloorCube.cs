using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class FloorCube : MonoBehaviour
	{
		//Config parameters
		public CubeTypes type = CubeTypes.Shrinking;
		public float shrinkStep = 0f;
		public float timeStep = 0f;

		public event Action<FloorCube> onRecordStart;
		public event Action<FloorCube> onRecordStop;

		//States
		public bool hasShrunk { get; set; } = false;

		public Vector2Int FetchGridPos()
		{
			Vector2Int roundedPos = new Vector2Int
				(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

			return roundedPos;
		}

		private void Update() 
		{
			CheckForVisualDisabling();
		}

		public void StartShrinking()
		{
			StartCoroutine(Shrink());
		}

		private IEnumerator Shrink()
		{
			hasShrunk = true;
			Vector3 targetScale = new Vector3(0, 0, 0);

			onRecordStart(this);

			for (int i = 0; i < (2.5 / shrinkStep); i++)
			{
				transform.localScale = 
					Vector3.Lerp(transform.localScale, targetScale, shrinkStep);
				yield return new WaitForSeconds(timeStep);
			}

			onRecordStop(this);
		}

		private void CheckForVisualDisabling()
		{
			if(type != CubeTypes.Shrinking) return;

			if(transform.localScale.x < .05f) 
				GetComponent<MeshRenderer>().enabled = false;
			else GetComponent<MeshRenderer>().enabled = true;
		}

		public CubeTypes FetchType()
		{
			return type;
		}
	}
}
