using Qbism.SpriteAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Shapies
{
	public class ShapieRefHolder : MonoBehaviour
	{
		//Config parameters
		public ShapieSoundHandler soundHandler;
		public Animator bodyAnimator, eyesAnimator, mouthAnimator;
		public ExpressionHandler expresHandler;
		public ShapieAnimator shapieAnimator;
		public SpriteEyesAnimator spriteEyesAnimator;
		public SpriteMouthAnimator spriteMouthAnimator;
		public SkinnedMeshRenderer bodyMesh, legsMesh;
	}
}
