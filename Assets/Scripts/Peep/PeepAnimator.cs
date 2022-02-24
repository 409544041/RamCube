using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Qbism.Peep
{
	public class PeepAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float idleChangeChance = .5f;
		[SerializeField] float idleAnimTransDur = .25f;
		[SerializeField] PeepRefHolder refs;

		//States
		float moveSpeed;
		Vector2 idleTimeMinMax;
		float idleType = 0;
		float idleTimer = 0;
		float timeToIdle;

		private void Start()
		{
			SetIdleMinMax();
			timeToIdle = Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
		}

		private void Update()
		{
			moveSpeed = refs.agent.velocity.magnitude;
			HandleIdleTimer();
			refs.animator.SetFloat("MoveSpeed", moveSpeed);
			refs.animator.SetFloat("IdleType", idleType);
		}

		private void HandleIdleTimer()
		{
			if (moveSpeed < .1f && idleType == 0) idleTimer += Time.deltaTime;
			if (idleTimer > timeToIdle)
			{
				RollForIdleAnimChange();
				idleTimer = 0;
				timeToIdle = Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
			}
		}

		private void SetIdleMinMax()
		{
			var idleMin = refs.idleState.idleTimeMinMax.x / 2;
			var idleMax = refs.idleState.idleTimeMinMax.y / 2;
			idleTimeMinMax = new Vector2(idleMin, idleMax);
		}

		private void RollForIdleAnimChange()
		{
			var randomMax = 1 / idleChangeChance;
			var roll = Random.Range(0, randomMax);
			if (roll <= 1) StartCoroutine(ChangeIdleAnim());
		}

		private IEnumerator ChangeIdleAnim()
		{
			float elapsedTime = 0;

			while (!Mathf.Approximately(idleType, 1))
			{
				elapsedTime += Time.deltaTime;
				var percentageComplete = elapsedTime / idleAnimTransDur;

				idleType = Mathf.Lerp(idleType, 1, percentageComplete);
				yield return null;
			}

			idleType = 1;
			refs.expressionHandler.SetAnnoyedExpression();
		}

		public void TriggerStartle()
		{
			refs.animator.SetTrigger("Startle");
		}

		private void ResetIdleType() //Called by animator
		{
			idleType = 0;
			idleTimer = 0;
			timeToIdle = Random.Range(idleTimeMinMax.x, idleTimeMinMax.y);
			refs.expressionHandler.SetNeutralExpression();
		}
	}
}
