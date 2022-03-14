using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Qbism.General;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using TMPro;
using Qbism.Saving;
using Qbism.MoveableCubes;
using Qbism.Environment;
using Qbism.Peep;

public class GameplayCoreRefHolder : MonoBehaviour
{
	[Header("Cameras")]
	public Camera cam;
	public CinemachineVirtualCamera gameCam;
	public PositionCenterpoint centerPoint;
	[Header("Logic")]
	public GameLogicRefHolder glRef;
	[Header("Music")]
	public MusicPlayer musicPlayer;
	public AudioSource source;
	public MusicFadeOut musicFader;
	[Header("Canvasses")]
	public Canvas dialogueCanvas;
	public CanvasGroup dialogueCanvasGroup;
	public TextMeshProUGUI characterNameText;
	public TextMeshProUGUI dialogueText;
	public MMFeedbacks dialogueNextButtonJuice;
	public Canvas bgCanvas;
	public CanvasGroup bgCanvasGroup;
	public Canvas gameplayCanvas;
	public CanvasGroup gameplayCanvasGroup;
	public InterfacePulser rewindPulser;
	public Canvas objectCanvas;
	public CanvasGroup objectCanvasGroup;
	public TextMeshProUGUI objectNameText;
	public TextMeshProUGUI objectSubText;
	public TextMeshProUGUI objectOwnerText;

	//Cache
	public PersistentRefHolder persRef { get; private set; }
	public BiomeVisualSwapper[] visualSwappers { get; private set; }

	public PeepRefHolder[] peeps;

	//TO DO: Add finish ref and cube refs

	private void Awake()
	{
		persRef = FindObjectOfType<PersistentRefHolder>();
		persRef.cam = cam;

		visualSwappers = FindObjectsOfType<BiomeVisualSwapper>();
		foreach (var swapper in visualSwappers)
		{
			swapper.progHandler = persRef.progHandler;
			swapper.matHandler = persRef.varMatHandler;
		}

		peeps = FindObjectsOfType<PeepRefHolder>();
		foreach (var peep in peeps)
		{
			peep.cam = cam;
		}
	}
}
