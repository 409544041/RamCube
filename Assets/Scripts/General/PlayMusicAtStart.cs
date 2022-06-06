using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class PlayMusicAtStart : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioSource source;
		[SerializeField] float playDelay = .25f;

		void Start()
		{
			//with delay bc otherwise there's a loud pop when playing
			//audio while muted via mixer (Unity bug I think)
			source.PlayDelayed(playDelay);
		}
	}
}
