using Qbism.Control;
using Qbism.Cubes;
using Qbism.Dialogue;
using Qbism.Environment;
using Qbism.General;
using Qbism.MoveableCubes;
using Qbism.Objects;
using Qbism.Peep;
using Qbism.Rewind;
using Qbism.SceneTransition;
using Qbism.ScreenStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicRefHolder : MonoBehaviour
{
	public GameplayCoreRefHolder gcRef;
	public CamResizer camResizer;
	public ScreenStateManager screenStateMngr;
	public Timer levelTimer;
	[Header("Cube Handling")]
	public CubeHandler cubeHandler;
	public MoveableCubeHandler movCubeHandler;
	public WallHandler wallHandler;
	public FloorCubeChecker floorChecker;
	public FloorCubeCheckerMoveable floorMovChecker;
	[Header("Rewind Handling")]
	public RewindHandler rewindHandler;
	[Header("Scene Handling")]
	public WorldMapLoading mapLoader;
	public SceneHandler sceneHandler;
	[Header("Biome Visual Handling")]
	public BiomeVisualSwapper visualSwapper;
	public BiomeOverwriter overwriter;
	[Header("Dialogue Handling")]
	public DialogueManager dialogueManager;
	public DialogueFocuser dialogueFocuser;
	[Header("Object Collect Handling")]
	public ObjectCollectorAtEndSeq objColManager;
	[Header("Pathfinding")]
	public PeepNavPointManager navManager;
	public AstarPath pathFinder;
}
