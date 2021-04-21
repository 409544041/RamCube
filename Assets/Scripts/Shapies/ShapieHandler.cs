using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Shapies
{
	public class ShapieHandler : MonoBehaviour
	{
		//Config parameters
		[Header("Pushback")]
		[SerializeField] Transform pushBackTarget;
		[SerializeField] MMFeedbacks pushBackJuice;
		[Header("Eyes")]
		[SerializeField] Sprite[] eyeTextures;
		[SerializeField] float blinkDuration = .15f;
		[SerializeField] float minBlinkInterval = .3f, maxBlinkInterval = 1.5f;

		//Cache
		SpriteRenderer sRender;

		//States
		bool randomBlink = false;

		private void Awake() 
		{
			sRender = GetComponentInChildren<SpriteRenderer>();
		}

		public void pushBack()
		{
			pushBackJuice.Initialization();
			pushBackJuice.PlayFeedbacks();

			sRender.sprite = eyeTextures[1];
		}

		public void StartBlinking()
		{
			sRender.sprite = eyeTextures[0];
			randomBlink = true;
			StartCoroutine(Blink());
		}

		private IEnumerator Blink()
		{
			while(randomBlink)
			{
				yield return new WaitForSeconds(Random.Range(minBlinkInterval, maxBlinkInterval));
				sRender.sprite = eyeTextures[1];
				yield return new WaitForSeconds(blinkDuration);
				sRender.sprite = eyeTextures[0];
			}
		}
	}
}
