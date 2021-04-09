 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qbism.Cubes;
using Qbism.Saving;

namespace Qbism.General
{
	public class SerpentMovement : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Transform head = null;
		[SerializeField] Transform[] segments = null;
		[SerializeField] float segmentSpacing = 1f;
		
		//Cache
		FinishCube finish;
		
		//States
		List<Vector3> breadcrumbs = null;
		bool isMoving = false;

		private void Awake() 
		{
			finish = FindObjectOfType<FinishCube>();
		}

		private void OnEnable() 
		{
			if(finish != null)
			{
				finish.onSetSerpentMove += SetMoving;
				finish.onShowSerpentSegments += ShowSegments;
			} 
		}

		void Start()
		{
			breadcrumbs = new List<Vector3>();
			breadcrumbs.Add(head.position);
			for (int i = 0; i < segments.Length; i++) 
				breadcrumbs.Add(segments[i].position);
		}

		void Update()
		{
			if (isMoving) FollowHead();
		}

		private void FollowHead()
		{
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
				segments[0].rotation = Quaternion.Slerp(Quaternion.LookRotation(breadcrumbs[0] - breadcrumbs[1]), Quaternion.LookRotation(head.position - breadcrumbs[0]), headDisplacement / segmentSpacing);

				for (int i = 1; i < segments.Length; i++)
				{
					pos = Vector3.Lerp(breadcrumbs[i + 1], breadcrumbs[i], headDisplacement / segmentSpacing);
					segments[i].position = pos;
					segments[i].rotation = Quaternion.Slerp(Quaternion.LookRotation(breadcrumbs[i] - breadcrumbs[i + 1]), Quaternion.LookRotation(breadcrumbs[i - 1] - breadcrumbs[i]), headDisplacement / segmentSpacing);
				}
			}
		}

		private void SetMoving(bool value)
		{
			isMoving = value;
		}

		private void ShowSegments()
		{
			SerpentProgress serpProg = FindObjectOfType<SerpentProgress>();
			for (int i = 0; i < segments.Length; i++)
			{
				MeshRenderer mRender = segments[i].GetComponentInChildren<MeshRenderer>();
				SpriteRenderer sRender = segments[i].GetComponentInChildren<SpriteRenderer>();

				if(!mRender || !sRender) Debug.LogError
					(segments[i] + " is missing either a meshrenderer or spriterenderer!");
				
				if(serpProg.serpentDataList[i] == true)
				{
					mRender.enabled = true;
					sRender.enabled = true;
				}
				else
				{
					mRender.enabled = false;
					sRender.enabled = false;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<FinishCube>()) other.transform.parent = transform;
		}

		private void OnDisable()
		{
			if (finish != null)
			{
				finish.onSetSerpentMove -= SetMoving;
				finish.onShowSerpentSegments -= ShowSegments;
			}
		}
	}
}