using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.AI;

public enum AIState { chasePlayer, searchingPlayer, patrol, runAway };

public class EnemyBasicAI : MonoBehaviour {
	private NavMeshAgent Agent;
	private EnemyPerception Perception;
	/*
	private float PerceptionDelay;
	private float AttackDistance;
	private float AttackTimeDelay;
	private float MinimalDistance;
	private float PatrolDistance;*/

	public float maxDistansToStartChase;
	public float reduceMinDistanceChaseValue;
	public int aiLivePoints;
	public int supremumAILivePoinstToFlie;
	public AIState aiState;
	public Vector3 lastAITargetPosition;

	List<Info> infos; 

	void Start() {
		maxDistansToStartChase = 20;
		reduceMinDistanceChaseValue = 0.2f;
		aiLivePoints = 100;
		supremumAILivePoinstToFlie = 20;
		lastAITargetPosition = gameObject.transform.position;

		Agent = GetComponent<NavMeshAgent>();
		Perception = GetComponent<EnemyPerception>();
		StartCoroutine(basicAI());
		List<Info> infos = new List<Info>(); 
		aiState = AIState.patrol;
	}


	
	void chasePlayer(Vector3 location) {
		lastAITargetPosition = location;
		Agent.SetDestination(location);
	}

	void changeAIState(AIState newState){
		aiState = newState;
	}
	
	void attack(Vector3 location) {
		/*Shoot at location*/
	}


	Vector3 lastNoticedPlayerPosition(){
		Vector3 returnPosition = Vector3.forward;
		float minDistanse = float.MaxValue;
		Vector3 temp;
		float nextPlayerDistance;
		foreach (Info inf in infos) {
			
			nextPlayerDistance = Vector3.Distance (gameObject.transform.position, inf.location);
			if (minDistanse > nextPlayerDistance) {
				minDistanse = nextPlayerDistance;
				returnPosition = inf.location;
			}
		}
		return returnPosition;
	}
		

	bool noticedPlayers(){
		foreach (Info inf in infos) {
			if (inf.message == AIPerceptionInformation.heardPlayer || 
				inf.message == AIPerceptionInformation.seenPlayer) {
				return true;
			}
		}
		return false;
	}


	void makeDecisionWhenNoticedPlayer(Vector3 target){
		if (Vector3.Distance (gameObject.transform.position, target) <= maxDistansToStartChase &&
		    supremumAILivePoinstToFlie < aiLivePoints) {
			//print ("gonie playera");
			chasePlayer (target);
			changeAIState (AIState.chasePlayer);
		} else {
			if (supremumAILivePoinstToFlie >= aiLivePoints) {
				//run away ToDo
				changeAIState (AIState.runAway);
			} else {
				//player too far to engage
				changeAIState (AIState.patrol);
			}
		}
	}
		
	
	void updatePerception() {
		infos = Perception.getInfos();
	}
	
	private IEnumerator basicAI() {

		Vector3 aiTargetLocation;

		while (true) {
			yield return new WaitForSeconds(0.5f);
			updatePerception ();

			if (noticedPlayers ()) {
				aiTargetLocation = lastNoticedPlayerPosition ();

				makeDecisionWhenNoticedPlayer (aiTargetLocation);

			} else if (aiState == AIState.chasePlayer) {
				//going to last indicated point
				changeAIState (AIState.searchingPlayer);
				maxDistansToStartChase -= reduceMinDistanceChaseValue;
			}


		}

	}	


	
	
	
}