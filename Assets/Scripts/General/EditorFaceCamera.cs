using Qbism.Peep;
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
		[SerializeField] PeepRefHolder peepRefs;

		//Cache
		Camera cam;

		private void Awake()
		{
			if (peepRefs != null) cam = peepRefs.cam;
			else cam = Camera.main; //TO DO: switch to cam ref once we have pin refs
			transform.forward = cam.transform.forward;
		}

		private void Update()
		{
			if (updateAlign && Application.isPlaying) transform.forward = cam.transform.forward;
		}
	}
}

