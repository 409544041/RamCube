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
	public CubeRefHolder[] cubeRefs { get; private set; }
	public FloorCube[] floorCubes { get; private set; }
	public CubeShrinker[] cubeShrinkers { get; private set; }
	public FloorComponentAdder[] floorCompAdders { get; private set; }

	//TO DO: assign floorcubes/cubeshrinkers from across cuberefs and finishref

	private void Awake()
	{
		List<FloorCube> floorCubeList = new List<FloorCube>();
		List<MoveableCube> movCubeList = new List<MoveableCube>();
		List<FloorComponentAdder> floorCompList = new List<FloorComponentAdder>();
		
		GetSetPersistentRef();

		GetSetPeepRef();

		GetSetFinishRef(floorCubeList);

		GetSetCubeRefs(floorCubeList, movCubeList, floorCompList);

		GetSetVisualSwappers();

		walls = GameObject.FindGameObjectsWithTag("Wall");

		GetSetShrinker();

		movCubes = movCubeList.ToArray();
		floorCubes = floorCubeList.ToArray();
		floorCompAdders = floorCompList.ToArray();
	}

	private void GetSetPersistentRef()
	{
		persRef = FindObjectOfType<PersistentRefHolder>();
		persRef.cam = cam;
		persRef.gcRef = this;
		persRef.glRef = glRef;
	}

	private void GetSetPeepRef()
	{
		peeps = FindObjectsOfType<PeepRefHolder>();
		foreach (var peep in peeps)
		{
			peep.cam = cam;
		}
	}

	private void GetSetFinishRef(List<FloorCube> floorCubeList)
	{
		finishRef = FindObjectOfType<FinishRefHolder>();
		finishRef.gcRef = this;
		finishRef.persRef = persRef;
		finishRef.cam = cam;
		floorCubeList.Add(finishRef.floorCube);
	}

	private void GetSetCubeRefs(List<FloorCube> floorCubeList, List<MoveableCube> movCubeList,
		List<FloorComponentAdder> floorCompList)
	{
		cubeRefs = FindObjectsOfType<CubeRefHolder>();
		foreach (var cube in cubeRefs)
		{
			cube.gcRef = this;
			if (cube.floorCube != null) floorCubeList.Add(cube.floorCube);
			if (cube.movCube != null) movCubeList.Add(cube.movCube);
			if (cube.floorCompAdder != null) floorCompList.Add(cube.floorCompAdder);
		}
	}

	private void GetSetVisualSwappers()
	{
		visualSwappers = FindObjectsOfType<BiomeVisualSwapper>();
		foreach (var swapper in visualSwappers)
		{
			swapper.progHandler = persRef.progHandler;
			swapper.matHandler = persRef.varMatHandler;
		}
	}

	private void GetSetShrinker()
	{
		//cant do shrinker via refs bc old walls also have shrinkers and no ref
		cubeShrinkers = FindObjectsOfType<CubeShrinker>();
		foreach (var shrinker in cubeShrinkers)
		{
			shrinker.handler = glRef.cubeHandler;
		}
	}
}
