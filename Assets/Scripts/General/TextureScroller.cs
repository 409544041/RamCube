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
		public Renderer rendererToScroll;
		public float scrollSpeedX, scrollSpeedY;
		public bool intervalScroll = false;
		public float maxOffsetX, maxOffsetY;
		public float interval;

		//States
		Vector2 offSet;
		bool active = true;
		float matOffsetX, matOffsetY;

		private void Update() 
		{
			if (active)
			{
				offSet = new Vector2(scrollSpeedX, scrollSpeedY) * Time.deltaTime;
				rendererToScroll.material.mainTextureOffset += offSet;
				matOffsetX = rendererToScroll.material.mainTextureOffset.x;
				matOffsetY = rendererToScroll.material.mainTextureOffset.y;
			}

			if (intervalScroll && active)
			{
				if ((scrollSpeedX != 0 && Mathf.Abs(matOffsetX) >= maxOffsetX) || 
					(scrollSpeedY != 0 && Mathf.Abs(matOffsetY) >= maxOffsetY))
				{
					active = false;
					offSet = Vector2.zero;
					rendererToScroll.material.mainTextureOffset = Vector2.zero;
					StartCoroutine(Interval());
				}
			}
		}

		private IEnumerator Interval()
		{
			yield return new WaitForSeconds(interval);
			active = true;
		}

		private void OnDisable() 
		{
			rendererToScroll.material.mainTextureOffset = new Vector2(0, 0);
		}
	}
}
