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

		//Cache
		Camera cam;

		private void Start()
		{
			cam = Camera.main;
			transform.forward = cam.transform.forward;
		}

		private void Update()
		{
			if (updateAlign && Application.isPlaying) transform.forward = cam.transform.forward;
		}
	}
}

