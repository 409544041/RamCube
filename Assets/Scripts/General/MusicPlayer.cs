using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class MusicPlayer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool mute;
		[SerializeField] MusicTracksScripOb musicSO;

		//Cache
		AudioSource source;

		//States
		float loopLength, loopEnd;

		private void Awake() 
		{
			source = GetComponent<AudioSource>();

			int i = Random.Range(0, musicSO.musicData.Length);
			loopLength = musicSO.musicData[i].loopLength;
			loopEnd = musicSO.musicData[i].loopEnd;
			source.clip = musicSO.musicData[i].track;
		}

		private void Start() 
		{
			if (!mute) source.Play();
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
				{
					source.timeSamples -= Mathf.RoundToInt(loopLength * source.clip.frequency);
				}
			}
		}

		private void GetSampleInfo() //Call in start to find out this data for new tracks
		{
			foreach (var data in musicSO.musicData)
			{
				var clipSampleDur = data.track.samples;
				print(data.track.name + "'s sample duration = " + clipSampleDur 
					+ " and it's sampling rate = " + data.track.frequency);
			}
		}
	}
}