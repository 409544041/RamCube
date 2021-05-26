using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class MistScroller : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float scrollSpeed = 1f;
		[SerializeField] Vector2 scrollDirection;

		//States
		Vector2 offSet;
		Material myMaterial;

		private void Awake()
		{
			myMaterial = GetComponent<Renderer>().material;
		}

		private void Start()
		{
			offSet = new Vector2(scrollDirection.x * scrollSpeed,
				scrollDirection.y * scrollSpeed);
		}

		private void Update() 
		{
			myMaterial.mainTextureOffset += offSet * Time.deltaTime;
		}
	}
}
