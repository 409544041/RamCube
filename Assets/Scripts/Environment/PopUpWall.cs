using System.Collections;
using System.Collections.Generic;
using Qbism.SpriteAnimations;
using UnityEngine;

namespace Qbism.Environment
{
	public class PopUpWall : MonoBehaviour
	{
		//Config parameters
		[SerializeField] GameObject wallMesh = null;
		[SerializeField] float popUpHeight = .5f;
		[SerializeField] float upwardDuration = .5f, downwardDuration = 1f;
		[SerializeField] float downwardDelay = .5f;

		//Cache
		PopUpWallJuicer juicer;
		ExpressionHandler exprHandler;

		//States
		Vector3 startPos;
		bool goingUp = false, goingDown = false;
		Vector3 popUpTarget;

		private void Awake() 
		{
			juicer = GetComponent<PopUpWallJuicer>();
			exprHandler = GetComponent<ExpressionHandler>();
		}

		private void Start() 
		{
			startPos = wallMesh.transform.position;
			popUpTarget = new Vector3(transform.position.x, 
				transform.position.y + popUpHeight, transform.position.z);
		}

		public void InitiatePopUp()
		{
			if (!goingUp) StartCoroutine(PopUp());
		}

		private IEnumerator PopUp()
		{
			goingUp = true;
			goingDown = false;

			float speed = (popUpHeight - startPos.y) / upwardDuration;

			exprHandler.SetFace(Expressions.toothyLaugh, -1);

			while (goingUp)
			{
				wallMesh.transform.position = Vector3.MoveTowards(wallMesh.transform.position, 
					popUpTarget, speed * Time.deltaTime);

				if (Vector3.Distance(wallMesh.transform.position, popUpTarget) < 0.01f)
				{
					goingUp = false;
					wallMesh.transform.position = popUpTarget;
					StartCoroutine(PopDown());
				}
					
				yield return null;
			}

			juicer.TriggerPopUpJuice();
		}

		private IEnumerator PopDown()
		{
			yield return new WaitForSeconds(downwardDelay);
			goingDown = true;

			exprHandler.SetFace(Expressions.looking, -1);

			float speed = (popUpHeight - startPos.y) / downwardDuration;

			while (goingDown)
			{
				wallMesh.transform.position = Vector3.MoveTowards(wallMesh.transform.position,
					startPos, speed * Time.deltaTime);

				if (Vector3.Distance(wallMesh.transform.position, startPos) < 0.01f)
				{
					goingDown = false;
					wallMesh.transform.position = startPos;
				}

				yield return null;
			}

			exprHandler.SetFace(Expressions.calm, -1);
		}
	}
}
