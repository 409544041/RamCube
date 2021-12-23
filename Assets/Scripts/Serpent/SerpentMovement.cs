 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using System;

namespace Qbism.Serpent
{
	public class SerpentMovement : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Transform head = null;
		[SerializeField] SerpentSegmentHandler segHandler;
		public float segmentSpacing = 1f;
		[Header ("Juice")]
		[SerializeField] AudioClip pickupClip;
		[SerializeField] AudioSource source;
		
		//Cache
		FinishEndSeqHandler finishEndSeq = null;
		
		//States
		List<Vector3> breadcrumbs = null;
		bool isMoving = false;
		public Transform[] segments = null;
		bool firstCrumbs = false;

		//Actions, events, delegates etc
		public event Action onTriggerPlayerAudio;

		private void Awake() 
		{
			finishEndSeq = FindObjectOfType<FinishEndSeqHandler>();

			breadcrumbs = new List<Vector3>();
		}

		private void OnEnable() 
		{
			if (finishEndSeq != null) finishEndSeq.onSetSerpentMove += SetMoving;
		}

        private void Start()
        {
			var serpMapHandler = GetComponent<SerpentMapHandler>();

		//	if (serpMapHandler != null) segments = segHandler.PrepareSegmentsInMap();
			/*else*/ segments = segHandler.segments;
		}

        void Update()
		{
			if (isMoving) FollowHead();
		}

		private void FollowHead()
		{
			if (!firstCrumbs)
			{
				breadcrumbs.Add(head.position);

				for (int i = 0; i < segments.Length; i++)
					breadcrumbs.Add(segments[i].position);
				
				firstCrumbs = true;
			}

			float headDisplacement = (head.position - breadcrumbs[0]).magnitude;

			if (headDisplacement >= segmentSpacing)
			{
				breadcrumbs.RemoveAt(breadcrumbs.Count - 1);
				breadcrumbs.Insert(0, head.position);
				headDisplacement = headDisplacement % segmentSpacing;
			}

			if (headDisplacement != 0)
			{
				Vector3 pos = Vector3.Lerp(breadcrumbs[1], breadcrumbs[0], headDisplacement / segmentSpacing);
				segments[0].position = pos;
				segments[0].rotation = Quaternion.Slerp(Quaternion.LookRotation(breadcrumbs[0] - breadcrumbs[1]), 
					Quaternion.LookRotation(head.position - breadcrumbs[0]), headDisplacement / segmentSpacing);

				for (int i = 1; i < segments.Length; i++)
				{
					pos = Vector3.Lerp(breadcrumbs[i + 1], breadcrumbs[i], headDisplacement / segmentSpacing);
					segments[i].position = pos;
					segments[i].rotation = Quaternion.Slerp(Quaternion.LookRotation(breadcrumbs[i] - breadcrumbs[i + 1]), 
						Quaternion.LookRotation(breadcrumbs[i - 1] - breadcrumbs[i]), headDisplacement / segmentSpacing);
				}
			}
		}

		public void SetMoving(bool value)
		{
			isMoving = value;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				other.transform.parent = transform;
				onTriggerPlayerAudio();
				source.PlayOneShot(pickupClip);
			} 		
		}

		private void OnDisable()
		{
			if (finishEndSeq != null) finishEndSeq.onSetSerpentMove -= SetMoving;
		}
	}
}