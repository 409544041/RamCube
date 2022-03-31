using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using Qbism.Saving;
using Qbism.General;

namespace Qbism.Serpent
{
	public class SerpCoreRefHolder : MonoBehaviour
	{
		//Config parameters
		public Camera cam;
		public CinemachineVirtualCamera serpCam;
		public MusicFadeOut musicFader;
		[Header("Logic")]
		public SerpLogicRefHolder slRef;
		[Header("Canvas")]
		public Canvas serpScreenCanvas;
		public CanvasGroup serpScreenCanvasGroup;
		public Image[] objSlotElements;
		public Canvas dialogueCanvas;
		public CanvasGroup dialogueCanvasGroup;
		public TextMeshProUGUI characterNameText;
		public TextMeshProUGUI dialogueText;
		public MMFeedbacks dialogueNextButtonJuice;
		public Canvas bgCanvas;
		public CanvasGroup bgCanvasGroup;
		[Header("Serpent")]
		public SerpentSegmentHandler segHandler;

		//Cache
		public SegmentRefHolder[] segRefs { get; private set; }
		public PersistentRefHolder persRef { get; private set; }

		private void Awake()
		{
			persRef = FindObjectOfType<PersistentRefHolder>();
			persRef.scRef = this;
			persRef.cam = cam;

			segRefs = FindObjectsOfType<SegmentRefHolder>();
			foreach (var segRef in segRefs)
			{
				segRef.scRef = this;
				segRef.cam = cam;
			}
		}
	}
}
