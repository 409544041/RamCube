using Qbism.Peep;
using Qbism.Serpent;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	public class FaceCamera : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool updateAlign;
		[SerializeField] PeepRefHolder peepRef;
		[SerializeField] LevelPinRefHolder pinRef;
		[SerializeField] SegmentRefHolder segRef;

		//Cache
		public Camera cam { get; set; }

		private void Awake()
		{
			if (peepRef != null) cam = peepRef.cam;
			else if (pinRef != null) cam = pinRef.mcRef.cam;
			else if (segRef != null) cam = segRef.cam;
			else cam = Camera.main;
		}

		private void Start()
		{
			if (cam != null) transform.forward = cam.transform.forward;
		}

		public void FaceToCam()
		{
			transform.forward = cam.transform.forward;
		}

		private void Update()
		{
			if (updateAlign) transform.forward = cam.transform.forward;
		}
	}
}

