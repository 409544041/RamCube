using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Objects
{
	public class SegObjectRefHolder : MonoBehaviour
	{
		//Config parameters
		public M_Objects m_objects;
		public Renderer[] meshes;
		public Transform meshTrans;
		[Header("Juice")]
		public SegmentObjectJuicer juicer;
		public MMFeedbacks spawnJuice;
		[Header("UI")]
		public Canvas canvas;
		public CanvasGroup canvasGroup;
	}
}
