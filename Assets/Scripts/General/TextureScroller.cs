using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.General
{
	public class TextureScroller : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float scrollSpeed = 7f;
		[SerializeField] Vector2 maxOffset = new Vector2(1, 0);

		//Cache
		CubeHandler handler;

		//States
		Vector2 offSet;
		Material myMaterial;

		private void Awake()
		{
			handler = FindObjectOfType<CubeHandler>();
			myMaterial = GetComponent<Renderer>().material;
		}

		private void OnEnable()
		{
			if (handler != null) handler.onLand += InitiateScroll;
		}

		void Start()
		{
			offSet = new Vector2(-scrollSpeed, 0);
		}

		private void InitiateScroll()
		{
			StartCoroutine(ScrollTexture());
		}

		private IEnumerator ScrollTexture() //Used in action
		{
			while (Mathf.Abs(myMaterial.mainTextureOffset.x) < maxOffset.x)
			{
				myMaterial.mainTextureOffset += offSet * Time.deltaTime;
				yield return null;
			}

			myMaterial.mainTextureOffset = new Vector2(0, 0);
		}

		private void OnDisable()
		{
			if (handler != null) handler.onLand -= InitiateScroll;
		}
	}
}
