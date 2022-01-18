using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SerpentCollHandler : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioSource source;
		[SerializeField] AudioClip pickupClip;

		//Actions, events, delegates etc
		public event Action onTriggerPlayerAudio;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				other.transform.parent = transform;
				onTriggerPlayerAudio();
				source.PlayOneShot(pickupClip);
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			var collHandler = collision.gameObject.GetComponent<ISerpentCollHandler>();

			if (collHandler != null) collHandler.HandleSerpentColl(transform.position);
		}

	}
}
