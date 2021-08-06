
using UnityEngine;

[CreateAssetMenu(fileName = "Music Tracks Scrip Obj", menuName = "ScriptableObjects/Music Tracks")]
public class MusicTracksScripOb : ScriptableObject
{
	//Config parameters
	public MusicData[] musicData;

	[System.Serializable]
	public class MusicData
	{
		public AudioClip track;
		public float loopLength;
		public float loopEnd;
	}
}
