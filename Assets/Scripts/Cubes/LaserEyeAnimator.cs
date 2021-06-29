using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserEyeAnimator : MonoBehaviour
	{
		//Cache
		Animator animator;

		private void Awake() 
		{
			animator = GetComponent<Animator>();
		}

		public void CloseEyes()
		{
			animator.SetBool("Open", false);
			animator.SetBool("Shooting", false);
		}

		public void OpenEyes()
		{
			animator.SetBool("Open", true);
			animator.SetBool("Shooting", false);
		}

		public void ShootyEyes()
		{
			animator.SetBool("Shooting", true);
		}
	}
}
