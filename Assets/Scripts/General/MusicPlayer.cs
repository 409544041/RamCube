using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class MusicPlayer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] MusicTracksScripOb musicSO;

		//Cache
		AudioSource source;
		MusicOrderHandler orderHandler;

		//States
		float loopLength, loopEnd;

		private void Awake() 
		{
			source = GetComponent<AudioSource>();
			orderHandler = FindObjectOfType<MusicOrderHandler>();

			if (orderHandler)
			{
				int i = orderHandler.currentTrack;
				loopLength = musicSO.musicData[i].loopLength;
				loopEnd = musicSO.musicData[i].loopEnd;
				source.clip = musicSO.musicData[i].track; 
			}
			else
			{
				int i = Random.Range(0, musicSO.musicData.Length);
				loopLength = musicSO.musicData[i].loopLength;
				loopEnd = musicSO.musicData[i].loopEnd;
				source.clip = musicSO.musicData[i].track;
			}
		}

		private void Start() 
		{
			source.Play();
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