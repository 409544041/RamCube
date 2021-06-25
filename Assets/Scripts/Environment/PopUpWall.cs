using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qbism.Environment
{
	public class PopUpWall : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject wallMesh = null;
		[SerializeField] float popUpHeight = .5f;
		[SerializeField] float upwardSpeed = 1f, downwardSpeed = .5f;
		[SerializeField] float downwardDelay = .5f;

		//States
		Vector3 startPos;
		bool goingUp = false, goingDown = false;
		Vector3 popUpTarget;

		private void Start() 
		{
			startPos = wallMesh.transform.position;
			popUpTarget = new Vector3(transform.position.x, 
				transform.position.y + popUpHeight, transform.position.z);
		}

		public void InitiatePopUp()
		{
			StartCoroutine(PopUp());
		}

		private IEnumerator PopUp()
		{
			goingUp = true;
			goingDown = false;

			while (goingUp)
			{
				wallMesh.transform.position = Vector3.MoveTowards(wallMesh.transform.position, 
					popUpTarget, upwardSpeed * Time.deltaTime);

				if (Vector3.Distance(wallMesh.transform.position, popUpTarget) < 0.01f)
				{
					goingUp = false;
					wallMesh.transform.position = popUpTarget;
					StartCoroutine(PopDown());
				}
					
				yield return null;
			}
		}

		private IEnumerator PopDown()
		{
			yield return new WaitForSeconds(downwardDelay);
			goingDown = true;

			while (goingDown)
			{
				wallMesh.transform.position = Vector3.MoveTowards(wallMesh.transform.position,
					startPos, downwardSpeed * Time.deltaTime);

				if (Vector3.Distance(wallMesh.transform.position, startPos) < 0.01f)
				{
					goingDown = false;
					wallMesh.transform.position = startPos;
				}

				yield return null;
			}
		}
	}
}
