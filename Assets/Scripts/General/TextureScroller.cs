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
		[SerializeField] Material material = null;
		[SerializeField] float scrollSpeed = 7f;
		[SerializeField] Vector2 maxOffset = new Vector2(1, 0);
		[SerializeField] bool scrollAlways = false;

		//Cache
		CubeHandler handler;

		//States
		Vector2 offSet;

		private void Awake()
		{
			handler = FindObjectOfType<CubeHandler>();
		}

		private void OnEnable()
		{
			if (handler != null) handler.onLand += InitiateScroll;
		}

		private void Update() 
		{
			offSet = new Vector2(-scrollSpeed, 0); 
			if (scrollAlways) material.mainTextureOffset += 
				offSet * Time.deltaTime;
		}

		private void InitiateScroll()
		{
			var booster = GetComponentInParent<BoostCube>();
			if (booster) StartCoroutine(ScrollTexture());
		}

		private IEnumerator ScrollTexture() //Used in action
		{
			while (Mathf.Abs(material.mainTextureOffset.x) < maxOffset.x)
			{
				material.mainTextureOffset += offSet * Time.deltaTime;
				yield return null;
			}

			material.mainTextureOffset = new Vector2(0, 0);
		}

		private void OnDisable()
		{
			if (handler != null) handler.onLand -= InitiateScroll;
		}
	}
}
