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
		[SerializeField] bool intervalScroll = false;
		[SerializeField] float maxOffsetX, maxOffsetY;
		[SerializeField] float interval;

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
				Debug.Log("Offset.x = " + matOffsetX + " and MaxOffsetX = " + maxOffsetX);

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
			Debug.Log("In Interval");
			yield return new WaitForSeconds(interval);
			active = true;
		}

		private void OnDisable() 
		{
			rendererToScroll.material.mainTextureOffset = new Vector2(0, 0);
		}
	}
}
