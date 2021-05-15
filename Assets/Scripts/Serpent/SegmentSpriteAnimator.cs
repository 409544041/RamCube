using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentSpriteAnimator : MonoBehaviour
	{
		//Config parameters
		public SegmentSpriteIDs spriteID;
		[SerializeField] float addedY;

		//Cache
		SpriteRenderer sRender;

		//States
		Vector3 sRenderNormalPos;
		Vector3 sRenderHighPos;

		private void Awake()
		{
			sRender = GetComponent<SpriteRenderer>();
		}

		private void Start()
		{
			sRenderNormalPos = transform.localPosition;
			sRenderHighPos = new Vector3(transform.localPosition.x, 
				transform.localPosition.y + addedY, transform.localPosition.z);
		}

		public void SetSprite(Sprite spriteToSet, bool normalPos)
		{
			if(normalPos) transform.localPosition = sRenderNormalPos;
			else transform.localPosition = sRenderHighPos;

			sRender.sprite = spriteToSet;
		}
	}
}
