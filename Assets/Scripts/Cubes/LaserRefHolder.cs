using Qbism.Cubes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRefHolder : MonoBehaviour
{
	[SerializeField] BoxCollider wallCollider;
	[SerializeField] LaserCube laser;
	[SerializeField] LaserJuicer juicer;
	[SerializeField] Rigidbody[] partsRB;
	[SerializeField] BoxCollider[] partsColl;
	[SerializeField] Animator[] partsAnimators;
	[SerializeField] LaserEyeAnimator eyeAnimator;
	[SerializeField] LaserMouthAnimator mouthAnimator;
}
