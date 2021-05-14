using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Qbism.Serpent
{
	public class SegmentAnimator : MonoBehaviour
	{
		//Config paramters
		[Header ("Spawn Juice")]
		[SerializeField] MMFeedbacks spawnJuice = null;
		[Header ("Landing Squish")]
		[SerializeField] MMFeedbacks squishAnim = null;
		[Header ("Animation")]
		[SerializeField] Animator animator = null;
		[SerializeField] float spawnAnimDelay = .2f, lookAroundAnimDelay = 0f, 
			lookUpAnimDelay = 0f, happyWiggleAnimDelay = 0f;

		//States
		bool firstHop = true;
		float spawnWiggleTime = 0f;
		float lookAroundTime = 0f;
		float lookUpTime = 0f;
		float happyWiggleTime = 0f;

		private void Start() 
		{
			GetAnimationClipTimes();
			StartCoroutine(TriggerSpawnSequence());
		}

		private void GetAnimationClipTimes()
		{
			AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
			foreach (AnimationClip clip in clips)
			{
				if(clip.name == "Segment Spawn Wiggle") spawnWiggleTime = clip.length;
				else if(clip.name == "Segment Look Around") lookAroundTime = clip.length;
				else if(clip.name == "Segment Look Up") lookUpTime = clip.length;
				else if(clip.name == "Segment Happy Wiggle") happyWiggleTime = clip.length;
			}
		}

		private IEnumerator TriggerSpawnSequence()
		{
			spawnJuice.Initialization();
			spawnJuice.PlayFeedbacks();
			yield return new WaitForSeconds(spawnAnimDelay);
			animator.SetTrigger("SpawnWiggle");
		}

		private IEnumerator TriggerLookAround()
		{
			yield return new WaitForSeconds(lookAroundAnimDelay);
			animator.SetTrigger("LookAround");
		}

		private IEnumerator TriggerLookUp()
		{
			//Take the player landing + squish reaction duration into account for look up delay
			yield return new WaitForSeconds(lookUpAnimDelay);
			animator.SetTrigger("LookUp");
		}

		private IEnumerator TriggerHappyWiggle()
		{
			yield return new WaitForSeconds(happyWiggleAnimDelay);
			animator.SetTrigger("HappyWiggle");
		}

		private void TriggerSquishAnim()
		{
			squishAnim.Initialization();
			squishAnim.PlayFeedbacks();
		}
	}
}
