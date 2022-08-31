using Qbism.Cubes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRefHolder : MonoBehaviour
{
	//Config parameters
	public BoxCollider wallCollider;
	public DetectionLaser detector;
	public LaserJuicer juicer;
	public Rigidbody[] partsRB;
	public BoxCollider[] partsColl;
	public Animator[] partsAnimators;
	public LaserEyeAnimator eyeAnimator;
	public LaserMouthAnimator mouthAnimator;

	//Cache
	public GameplayCoreRefHolder gcRef { get; set; }
}
