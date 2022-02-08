using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TEMPNavMeshTest : MonoBehaviour
{
	//Config parameters
	[SerializeField] Transform navTarget;
	[SerializeField] NavMeshAgent agent;

	private void Start()
	{
		StartCoroutine(MoveAgent());
	}

	private IEnumerator MoveAgent()
	{
		yield return new WaitForSeconds(2);
		agent.destination = navTarget.position;
	}
}
