using MoreMountains.Feedbacks;
using Pathfinding;
using Qbism.General;
using Qbism.MoveableCubes;
using Qbism.PlayerCube;
using Qbism.Rewind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class CubeRefHolder : MonoBehaviour
	{
		//Config parameters
		[Header("Standard")]
		public FloorCube floorCube;
		public CubePositioner cubePos;
		public CubeShrinker cubeShrink;
		public TimeBody timeBody;
		public Renderer mesh;
		public Renderer shrinkMesh;
		public LineRenderer lineRender;
		[Header("Effector")]
		public BoostCube boostCube;
		public Transform boostRayOrigin;
		public Transform boostDirTrans;
		public TurningCube turnCube;
		public EditorFlipArrows arrowFlip;
		public StaticCube staticCube;
		public GameObject staticFace;
		public GameObject staticShrinkingFace;
		[Header("Moveables")]
		public MoveableCube movCube;
		public MoveableEffector movEffector;
		public Renderer movFaceMesh;
		public FloorComponentAdder floorCompAdder;
		public BoostComponentAdder boostCompAdder;
		public TurnComponentAdder turnCompAdder;
		public StaticComponentAdder staticCompAdder;
		[Header("UI")]
		public CubeUIHandler cubeUI;
		public LineRenderer uiLineRender;
		public Transform[] uiLineTargets;
		public GameObject uiElement;
		[Header("Juice")]
		public RewindJuicer rewindJuicer;
		public MMFeedbacks turnJuice;
		public MMFeedbacks staticFaceShrinkJuice;
		public PlayerCubeBoostJuicer boostJuicer;

		//Cache
		public GameplayCoreRefHolder gcRef { get; set; }
	}
}
