using Qbism.Cubes;
using Qbism.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.MoveableCubes
{
	public class BoostComponentAdder : MonoBehaviour
	{
		//Config parameters
		[Header("Boost Component")]
		[SerializeField] LayerMask boostMaskPlayer;
		[SerializeField] LayerMask boostMaskMoveable;
		[Header("Texture Scroller Component")]
		[SerializeField] Renderer rendererToScroll;
		[SerializeField] float scrollSpeedX, scrollSpeedY;
		[SerializeField] bool intervalScroll = false;
		[SerializeField] float maxOffsetX, maxOffsetY;
		[SerializeField] float interval;
		[SerializeField] CubeRefHolder refs;

		public void AddBoostComponent(GameObject cube)
		{
			BoostCube newBoost = cube.AddComponent<BoostCube>();
			newBoost.boostMaskPlayer = boostMaskPlayer;
			newBoost.boostMaskMoveable = boostMaskMoveable;
			refs.boostCube = newBoost;
			newBoost.refs = refs;

			TextureScroller texScroller = cube.AddComponent<TextureScroller>();
			texScroller.rendererToScroll = rendererToScroll;
			texScroller.scrollSpeedX = scrollSpeedX;
			texScroller.scrollSpeedY = scrollSpeedY;
			texScroller.intervalScroll = intervalScroll;
			texScroller.maxOffsetX = maxOffsetX;
			texScroller.maxOffsetY = maxOffsetY;
			texScroller.interval = interval;
			refs.texScroller = texScroller;
		}
	}
}
