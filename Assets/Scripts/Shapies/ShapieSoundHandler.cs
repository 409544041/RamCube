using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Shapies
{
	public class ShapieSoundHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip[] gibberishClips;
		[SerializeField] AudioSource source;
		public Vector2 slowIntervalMinMax, fastIntervalMinMax; 
		[SerializeField] Vector2 pitchMinMax;

		//States
		public bool loopGibberish { get; set; } = false;
		float looptimer = 0;
		bool pitchSet = false;
		float pitch = 1;
		public Vector2 currentIntervalMinMax { get; set; }

		private void Start() 
		{
			looptimer = Random.Range
				(slowIntervalMinMax.x, slowIntervalMinMax.y);
		}

		private void Update()
		{
			HandleLoopTimer();
		}

		private void HandleLoopTimer()
		{
			if (loopGibberish)
			{
				looptimer += Time.deltaTime;
				var loopInterval = Random.Range
					(currentIntervalMinMax.x, currentIntervalMinMax.y);

				if (looptimer >= loopInterval)
				{
					PlaySigleGibberish();
					looptimer = 0;
				}
			}
		}

		public void PlayGibberish(bool continuous)
		{
			if (!pitchSet) pitch = Random.Range
				(pitchMinMax.x, pitchMinMax.y);

			pitchSet = true;

			if (continuous) loopGibberish = true;
			else PlaySigleGibberish();
		}

		private void PlaySigleGibberish()
		{
			source.pitch = Random.Range(.9f, 1.1f);
			int i = Random.Range(0, gibberishClips.Length);
			source.PlayOneShot(gibberishClips[i]);
		}
	}
}
