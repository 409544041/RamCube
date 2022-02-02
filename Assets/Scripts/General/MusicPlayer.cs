using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class MusicPlayer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MusicTracksScripOb musicSO;
		[SerializeField] MusicTracksScripOb levelCompSO;
		[SerializeField] float musicFadeOutDur = .5f;

		//Cache
		AudioSource source;
		MusicOrderHandler orderHandler;

		//States
		float loopLength, loopEnd;

		private void Awake()
		{
			source = GetComponent<AudioSource>();
			orderHandler = FindObjectOfType<MusicOrderHandler>();

			int i;

			if (orderHandler) i = orderHandler.currentTrack;
			else i = Random.Range(0, musicSO.musicData.Length);

			loopLength = musicSO.musicData[i].loopLength;
			loopEnd = musicSO.musicData[i].loopEnd;
			source.clip = musicSO.musicData[i].track;
		}

		private void Start()
		{
			if (!orderHandler.muteMusic) source.Play();
		}

		private void Update()
		{
			LoopLoopablePart();
		}

		private void LoopLoopablePart()
		{
			if (loopLength > 0 && loopEnd > 0)
			{
				//sec duration * sampling rate (frequency) = sample duration
				if (source.timeSamples >= loopEnd * source.clip.frequency)
					source.timeSamples -= Mathf.RoundToInt(loopLength * source.clip.frequency);
			}
		}

		public void InitiageLevelCompleteTrack()
		{
			StartCoroutine(PlayLevelCompTrack());
		}

		private IEnumerator PlayLevelCompTrack()
		{
			var startVol = source.volume;
			float elapsedTime = 0;

			while (!Mathf.Approximately(source.volume, 0))
			{
				elapsedTime += Time.deltaTime;
				var percentageComplet = elapsedTime / musicFadeOutDur;

				source.volume = Mathf.Lerp(startVol, 0, percentageComplet);

				yield return null;
			}

			source.clip = levelCompSO.musicData[0].track;
			loopLength = levelCompSO.musicData[0].loopLength;
			loopEnd = levelCompSO.musicData[0].loopEnd;

			source.volume = startVol;
			source.Play();
		}

		private void GetSampleInfo() //Call in start to find out this data for new tracks
		{
			foreach (var data in musicSO.musicData)
			{
				var clipSampleDur = data.track.samples;
				Debug.Log(data.track.name + "'s sample duration = " + clipSampleDur 
					+ " and it's sampling rate = " + data.track.frequency);
			}
		}

		private void OnDisable() 
		{
			if (orderHandler)
			{
				if (orderHandler.currentTrack == musicSO.musicData.Length - 1)
					orderHandler.currentTrack = 0;
				else orderHandler.currentTrack++;
			} 
		}
	}
}