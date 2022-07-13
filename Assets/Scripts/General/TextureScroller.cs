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
		[SerializeField] Renderer rendererToScroll;
		[SerializeField] float scrollSpeedX, scrollSpeedY;
		[SerializeField] bool scrollAlways = false;

		//States
		Vector2 offSet;

		private void Update() 
		{
			offSet = new Vector2(scrollSpeedX * Time.deltaTime, scrollSpeedY * Time.deltaTime); 
			if (scrollAlways) rendererToScroll.material.mainTextureOffset += 
				offSet * Time.deltaTime;
		}

		private void OnDisable() 
		{
			rendererToScroll.material.mainTextureOffset = new Vector2(0, 0);
		}
	}
}
