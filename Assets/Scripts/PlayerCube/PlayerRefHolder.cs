using Qbism.Cubes;
using Qbism.General;
using Qbism.Rewind;
using Qbism.SpriteAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.PlayerCube
{
	public class PlayerRefHolder : MonoBehaviour
	{
		//Config parameters
		[Header("Cube")]
		public PlayerCubeMover playerMover;
		public CubePositioner cubePos;
		[Header("Feed Forward")]
		public PlayerCubeFeedForward playerFF;
		public FeedForwardCube[] ffCubes;
		[Header("Animation")]
		public Animator animator;
		public PlayerAnimator playerAnim;
		public ExpressionHandler exprHandler;
		public SpriteRenderer browSprite;
		public Animator browAnimator;
		public SpriteBrowAnimator browAnim;
		public SpriteRenderer eyesSprite;
		public Animator eyesAnimator;
		public SpriteEyesAnimator eyesAnim;
		public SpriteRenderer mouthSprite;
		public Animator mouthAnimator;
		public SpriteMouthAnimator mouthAnim;
		[Header("Rewind")]
		public TimeBody timeBody;
		[Header("Farts")]
		public PlayerFartLauncher fartLauncher;
		[Header("Juice")]
		public AudioSource source;
		public AudioSource dopplerSource;
		public VisualsSwitch visualSwitch;
		public ScreenDistanceShrinker disShrinker;
		public ExplosionForce explForce;
		public PlayerCollHandler collHandler;
		public PlayerCubeFlipJuicer flipJuicer;
		public PlayerCubeBoostJuicer boostJuicer;
		public PlayerCubeTurnJuicer turnJuicer;
		public PlayerStunJuicer stunJuicer;
		public RewindJuicer rewindJuicer;
		public PlayerIntroJuicer introJuicer;
		public PlayerOutroJuicer outroJuicer;
		public PlayerFartJuicer fartJuicer;
		public FaceJuicer faceJuicer;

		//States
		public GameplayCoreRefHolder gcRef { get; set; }
		public Camera cam { get; set; }
	}
}
