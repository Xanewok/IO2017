using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

	public Transform goal;

	UnityEngine.AI.NavMeshAgent agent;

	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.destination = goal.position; 
	}



	void Update () {
		agent.SetDestination(goal.position); 
	}
}
