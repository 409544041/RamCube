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
using Qbism.Cubes;

public class GameplayCoreRefHolder : MonoBehaviour
{
	[Header("Cameras")]
	public Camera cam;
	public CinemachineVirtualCamera gameCam;
	public PositionCenterpoint centerPoint;
	[Header("Logic")]
	public GameLogicRefHolder glRef;
	public OutOfBounds[] outOfBounds;
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
	public ImageFader gameplayCanvasFader;
	public InterfacePulser rewindPulser;
	public Canvas objectCanvas;
	public CanvasGroup objectCanvasGroup;
	public TextMeshProUGUI objectNameText;
	public TextMeshProUGUI objectSubText;
	public TextMeshProUGUI objectOwnerText;

	//Cache
	public PersistentRefHolder persRef { get; private set; }
	public PeepRefHolder[] peeps { get; private set; }
	public BiomeVisualSwapper[] visualSwappers { get; private set; }
	public MoveableCube[] movCubes { get; private set; }
	public GameObject[] walls { get; private set; }

	public FinishRefHolder finishRef { get; private set; }


	//TO DO: Add finish ref and cube refs

	private void Awake()
	{
		persRef = FindObjectOfType<PersistentRefHolder>();
		persRef.cam = cam;
		persRef.gcRef = this;
		persRef.glRef = glRef;

		peeps = FindObjectsOfType<PeepRefHolder>();
		foreach (var peep in peeps)
		{
			peep.cam = cam;
		}

		finishRef = FindObjectOfType<FinishRefHolder>();
		finishRef.gcRef = this;
		finishRef.persRef = persRef;
		finishRef.cam = cam;

		visualSwappers = FindObjectsOfType<BiomeVisualSwapper>();
		foreach (var swapper in visualSwappers)
		{
			swapper.progHandler = persRef.progHandler;
			swapper.matHandler = persRef.varMatHandler;
		}

		movCubes = FindObjectsOfType<MoveableCube>(); //TO DO: add movcube reffers
		walls = GameObject.FindGameObjectsWithTag("Wall");
	}
}
