using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Qbism.PlayerCube;
using UnityEngine;

namespace Qbism.Serpent
{
	public class MotherDragonAnimator : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Animator animator;
		[SerializeField] MMFeedbacks flybyJuice, spawnJuice;

		//Cache
		PlayerAnimator playerAnim = null;

		//States
		float headSpawnDegrees;

		private void Awake()
		{
			playerAnim = FindObjectOfType<PlayerAnimator>();
		}

		public void ActivateFlyByJuice()
		{
			flybyJuice.Initialization();
			flybyJuice.PlayFeedbacks();
		}

		public void Spawn(float headSpawnDeg)
        {
			headSpawnDegrees = headSpawnDeg;

			int[] indexes = new int[] { 1, 2, 3, 4, 5 };
			SetWeights(indexes, 0);

			spawnJuice.Initialization();
			spawnJuice.PlayFeedbacks();

			animator.SetTrigger("SpawnWiggle");
        }

		private void SetWeights(int[] indexes, float weight)
        {
			for (int i = 0; i < indexes.Length; i++)
				animator.SetLayerWeight(indexes[i], weight); 
        }

		private void TriggerPlayerLanding() //Called from animation event
		{
			playerAnim.TriggerFall(false, "FallOnGround", headSpawnDegrees + 180, true);
			playerAnim.SetWithMother(true);
		}

		private void TriggerPlayerCuddle() // Called from animation event
        {
			playerAnim.TriggerCuddle();
        }
	}
}
