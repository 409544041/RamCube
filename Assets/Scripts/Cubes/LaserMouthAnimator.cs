using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserMouthAnimator : MonoBehaviour
	{
		//Cache
		Animator animator;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		public void HappyMouth()
		{
			animator.SetBool("Happy", true);
		}

		public void SadMouth()
		{
			animator.SetBool("Happy", false);
		}
	}
}
