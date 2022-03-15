using Qbism.Objects;
using Qbism.Serpent;
using Qbism.Shapies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Cinemachine;
using Qbism.Saving;
using Dreamteck.Splines;

namespace Qbism.Cubes
{
	public class FinishRefHolder : MonoBehaviour
	{
		//Config parameters
		[Header("Cube")]
		public FinishCube finishCube;
		public FloorCube floorCube;
		public CubePositioner cubePos;
		[Header("Meshes")]
		public Renderer mesh;
		public Renderer glowMesh;
		public NavmeshCut nmCutter;
		[Header("End Sequence")]
		public FinishEndSeqHandler endSeq;
		public Transform fartTarget;
		public CinemachineVirtualCamera closeUpCam;
		public Transform closeUpCamTarget;
		public CinemachineVirtualCamera endCam;
		[Header("Juice")]
		public FinishCubeJuicer finishJuicer;
		public ExplosionForce explForce;
		public AudioSource source;
		public Animator animator;
		[Header("Spawners")]
		public ShapieSpawner shapieSpawner;
		public SegmentSpawner segSpawner;
		public ObjectSpawner objSpawner;
		[Header("Serpent")]
		public SplineFollower follower;
		public SerpentMovement serpMovement;
		public SerpentSegmentHandler serpSegHandler;
		public SplineComputer spline;

		//Cache
		public GameplayCoreRefHolder gcRef { get; set; }
		public PersistentRefHolder persRef { get; set; }
		public Camera cam { get; set; }
	}
}
