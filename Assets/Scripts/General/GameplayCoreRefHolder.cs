using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Qbism.General;
using MoreMountains.Feedbacks;
using TMPro;
using Qbism.Saving;
using Qbism.MoveableCubes;
using Qbism.Environment;
using Qbism.Peep;
using Qbism.Cubes;
using Qbism.Rewind;
using Qbism.PlayerCube;
using Qbism.Serpent;
using Febucci.UI;

[ExecuteAlways]
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
	[Header("Gameplay Canvas")]
	public Canvas gameplayCanvas;
	public CanvasGroup gameplayCanvasGroup;
	public ImageFader gameplayCanvasFader;
	public InterfacePulser rewindPulser;
	[Header("Dialogue Canvas")]
	public Canvas dialogueCanvas;
	public CanvasGroup dialogueCanvasGroup;
	public TextMeshProUGUI characterNameText;
	public TextMeshProUGUI dialogueText;
	public MMFeedbacks dialogueNextButtonJuice;
	public Canvas bgCanvas;
	public CanvasGroup bgCanvasGroup;
	public TextAnimatorPlayer typeWriter;
	public MMFeedbacks textAppearJuice;
	public Camera gausCam;
	[Header("Object Canvas")]
	public Canvas objectCanvas;
	public CanvasGroup objectCanvasGroup;
	public TextMeshProUGUI objectNameText;
	public TextMeshProUGUI objectSubText;
	public TextMeshProUGUI objectOwnerText;
	[Header("Pause Canvas")]
	public OverlayMenuHandler pauseOverlayHandler;
	[Header("Settings Canvas")]
	public OverlayMenuHandler settingsOverlayHandler;
	public CanvasGroup settingsOverlayCanvasGroup;

	//Cache
	public PersistentRefHolder persRef { get; private set; }
	public PeepRefHolder[] peeps { get; private set; }
	public BiomeVisualSwapper[] visualSwappers { get; private set; }
	public MoveableCube[] movCubes { get; private set; }
	public GameObject[] walls { get; private set; }
	public FinishRefHolder finishRef { get; private set; } 
	public CubeRefHolder[] cubeRefs { get; private set; }
	public FloorCube[] floorCubes { get; private set; }
	public FloorComponentAdder[] floorCompAdders { get; private set; }
	public TimeBody[] timeBodies { get; private set; }
	public PlayerRefHolder pRef { get; private set; }
	public LaserRefHolder[] laserRefs { get; private set; }
	public SegmentRefHolder[] segRefs { get; private set; }
	public WallRefHolder[] wallRefs { get; private set; }

	private void Awake()
	{
		List<FloorCube> floorCubeList = new List<FloorCube>();
		List<MoveableCube> movCubeList = new List<MoveableCube>();
		List<FloorComponentAdder> floorCompList = new List<FloorComponentAdder>();
		List<TimeBody> timeBodyList = new List<TimeBody>();

		GetSetPersistentRef();
		GetSetPeepRef();
		GetSetFinishRef(floorCubeList);
		GetSetCubeRefs(floorCubeList, movCubeList, floorCompList, timeBodyList);
		GetSetVisualSwappers();
		GetSetPlayer(timeBodyList);
		GetSetLasers();
		GetSetSegments();
		GetWallRefs();

		walls = GameObject.FindGameObjectsWithTag("Wall");

		movCubes = movCubeList.ToArray();
		floorCubes = floorCubeList.ToArray();
		floorCompAdders = floorCompList.ToArray();
		timeBodies = timeBodyList.ToArray();
	}

	private void GetSetSegments()
	{
		segRefs = finishRef.serpSegHandler.segRefs;
		foreach (var segRef in segRefs)
		{
			segRef.gcRef = this;
			segRef.cam = cam;
		}
	}

	private void GetSetLasers()
	{
		laserRefs = FindObjectsOfType<LaserRefHolder>();
		foreach (var laser in laserRefs)
		{
			laser.gcRef = this;
		}
	}

	private void GetSetPlayer(List<TimeBody> timeBodyList)
	{
		pRef = FindObjectOfType<PlayerRefHolder>();
		pRef.gcRef = this;
		pRef.cam = cam;
		timeBodyList.Add(pRef.timeBody);
	}

	private void GetSetPersistentRef()
	{
		persRef = FindObjectOfType<PersistentRefHolder>();
		if (persRef != null)
		{
			persRef.cam = cam;
			persRef.gcRef = this;
			persRef.glRef = glRef;
		}
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
		List<FloorComponentAdder> floorCompList, List<TimeBody> timeBodyList)
	{
		cubeRefs = FindObjectsOfType<CubeRefHolder>();
		foreach (var cube in cubeRefs)
		{
			cube.gcRef = this;
			if (cube.floorCube != null) floorCubeList.Add(cube.floorCube);
			if (cube.movCube != null) movCubeList.Add(cube.movCube);
			if (cube.floorCompAdder != null) floorCompList.Add(cube.floorCompAdder);
			if (cube.timeBody != null) timeBodyList.Add(cube.timeBody);
		}
	}

	private void GetSetVisualSwappers()
	{
		visualSwappers = FindObjectsOfType<BiomeVisualSwapper>();
		foreach (var swapper in visualSwappers)
		{
			if (persRef != null)
			{
				swapper.progHandler = persRef.progHandler;
				swapper.matHandler = persRef.varMatHandler;
			}
		}
	}

	private void GetWallRefs()
	{
		wallRefs = FindObjectsOfType<WallRefHolder>();
		foreach (var wall in wallRefs)
		{
			wall.gcRef = this;
		}
	}
}
