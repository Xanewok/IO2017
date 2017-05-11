using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

/// <summary>
/// Glider ai.
/// He, who glides above the skies, is to be the one catches you in its eyes.
/// </summary>
public class GliderAi : MonoBehaviour {

	public float distanceTrigger = 4f;
	public float maxDistanceToStartChase = 20;
	public AIState aiState;
	public Vector3 lastAITargetPosition;
	public GameObject spawnOnTrigger;
	private NavMeshAgent agent;
	private EnemyPerception perception;
	private Status status;
	private List<Info> infos = new List<Info>();

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		perception = GetComponent<EnemyPerception>();
		status = GetComponent<Status>();

		lastAITargetPosition = transform.position;
		aiState = AIState.patrol;

		StartCoroutine(basicAI());
	}

	void chasePlayer(Vector3 location)
	{
		lastAITargetPosition = location;
		agent.SetDestination(lastAITargetPosition);
	}

	void changeAIState(AIState newState)
	{
		aiState = newState;
	}

	void onTriggered() {
		if (spawnOnTrigger != null) {
			Instantiate (spawnOnTrigger, transform.position, transform.rotation);
		}
		Destroy (gameObject);
	}

	Vector3 lastNoticedPlayerPosition()
	{
		Vector3 returnPosition = Vector3.forward;
		float minDistance = float.MaxValue;

		foreach (Info inf in infos)
		{
			float nextPlayerDistance = Vector3.Distance(gameObject.transform.position, inf.location);
			if (minDistance > nextPlayerDistance)
			{
				minDistance = nextPlayerDistance;
				returnPosition = inf.location;
			}
		}
		return returnPosition;
	}

	bool noticedPlayers()
	{
		foreach (Info inf in infos)
		{
			//Only seen, because he needs line of view.
			if (inf.message == AIPerceptionInformation.seenPlayer)
			{
				return true;
			}
		}
		return false;
	}

	void makeDecisionWhenNoticedPlayer(Vector3 target)
	{
		float distanceToTarget = Vector3.Distance(gameObject.transform.position, target);

		if (distanceToTarget <= maxDistanceToStartChase)
		{
			chasePlayer(target);
			changeAIState(AIState.chasePlayer);
			if (distanceToTarget < distanceTrigger) {
				onTriggered ();
			}
		}
		else
		{
				//player too far to engage
				changeAIState(AIState.patrol);
		}
	}

	void updatePerception()
	{
		infos = perception.getInfos();
	}

	private IEnumerator basicAI()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.1f);
			updatePerception();

			if (noticedPlayers())
			{
				Vector3 aiTargetLocation = lastNoticedPlayerPosition();

				makeDecisionWhenNoticedPlayer(aiTargetLocation);

			}
			else if (aiState == AIState.chasePlayer)
			{
				changeAIState(AIState.patrol);
			}
		}
	}

}
