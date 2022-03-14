using Qbism.Peep;
using Qbism.WorldMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.General
{
	[ExecuteInEditMode]
	public class EditorFaceCamera : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool updateAlign;
		[SerializeField] PeepRefHolder peepRef;
		[SerializeField] LevelPinRefHolder pinRef;

		//Cache
		Camera cam;

		private void Awake()
		{
			if (peepRef != null) cam = peepRef.cam;
			else if (pinRef != null) cam = pinRef.mcRef.cam;
			transform.forward = cam.transform.forward;
		}

		private void Update()
		{
			if (updateAlign && Application.isPlaying) transform.forward = cam.transform.forward;
		}
	}
}

