using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Shapies
{
	public class ShapieSpriteAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Sprite[] eyeTextures;
		[SerializeField] float blinkDuration = .15f;
		[SerializeField] float minBlinkInterval = 1.5f, maxBlinkInterval = 3f;

		//Cache
		SpriteRenderer sRender;

		//States
		bool randomBlink = false;

		private void Awake()
		{
			sRender = GetComponentInChildren<SpriteRenderer>();
		}

		private void SetEyes(int eyeIndex)
		{
			sRender.sprite = eyeTextures[eyeIndex];
		}

		private void StartBlinking()
		{
			sRender.sprite = eyeTextures[0];
			randomBlink = true;
			StartCoroutine(Blink());
		}

		private IEnumerator Blink()
		{
			while (randomBlink)
			{
				yield return new WaitForSeconds(Random.Range(minBlinkInterval, maxBlinkInterval));
				sRender.sprite = eyeTextures[1];
				yield return new WaitForSeconds(blinkDuration);
				sRender.sprite = eyeTextures[0];
			}
		}

		private void StopBlinking()
		{
			randomBlink = false;
		}
	}
}
