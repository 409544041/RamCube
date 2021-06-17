using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Cubes
{
	public class LaserAnimationHandler : MonoBehaviour
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
			print("setting closed");
		}

		public void OpenEyes()
		{
			animator.SetBool("Open", true);
			print("setting open");
		}

		public void Blink()
		{
			animator.SetTrigger("Blink");
		}
	}
}
