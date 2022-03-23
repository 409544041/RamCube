using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

namespace Qbism.Serpent
{
	public class SerpCoreRefHolder : MonoBehaviour
	{
		//Config parameters
		public Camera cam;
		public CinemachineVirtualCamera serpCam;
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
	}
}
