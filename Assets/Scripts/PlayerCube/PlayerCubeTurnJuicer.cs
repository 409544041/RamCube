using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerCubeTurnJuicer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip voiceClip;
		[SerializeField] AudioSource source;

		public void PlayTurningVoice()
		{
			source.PlayOneShot(voiceClip);
		}
	}
}
