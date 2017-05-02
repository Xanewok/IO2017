using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.AI;

public enum AIState { chasePlayer, searchingPlayer, patrol, runAway };

public class EnemyBasicAI : MonoBehaviour
{
    public float attackRange = 2.0f;
    public float maxDistanceToStartChase = 20;
    public float reduceMinDistanceChaseValue = 0.2f;
    public float hpEscapeThreshold = 20.0f;
    public AIState aiState;
    public Vector3 lastAITargetPosition;

    private NavMeshAgent agent;
    private EnemyPerception perception;
    private EnemyAttack attack;
    private Status status;
    private List<Info> infos = new List<Info>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        perception = GetComponent<EnemyPerception>();
        attack = GetComponent<EnemyAttack>();
        status = GetComponent<Status>();

        lastAITargetPosition = transform.position;
        aiState = AIState.patrol;

        StartCoroutine(basicAI());
    }

    void chasePlayer(Vector3 location)
    {
        lastAITargetPosition = location;
        agent.SetDestination(location);
    }

    void changeAIState(AIState newState)
    {
        aiState = newState;
    }

    void Attack(Vector3 location)
    {
        if (attack != null)
        {
            attack.Attack();
        }
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
            if (inf.message == AIPerceptionInformation.heardPlayer ||
                inf.message == AIPerceptionInformation.seenPlayer)
            {
                return true;
            }
        }
        return false;
    }

    void makeDecisionWhenNoticedPlayer(Vector3 target)
    {
        float distanceToTarget = Vector3.Distance(gameObject.transform.position, target);
        float health = status.getHealth();

        if (distanceToTarget <= maxDistanceToStartChase && health > hpEscapeThreshold)
        {
            chasePlayer(target);
            changeAIState(AIState.chasePlayer);

            if (distanceToTarget <= attackRange)
            {
                Attack(transform.position + transform.forward * attackRange);
            }
        }
        else
        {
            if (health <= hpEscapeThreshold)
            {
                //run away ToDo
                changeAIState(AIState.runAway);
            }
            else
            {
                //player too far to engage
                changeAIState(AIState.patrol);
            }
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
            yield return new WaitForSeconds(0.5f);
            updatePerception();

            if (noticedPlayers())
            {
                Vector3 aiTargetLocation = lastNoticedPlayerPosition();

                makeDecisionWhenNoticedPlayer(aiTargetLocation);

            }
            else if (aiState == AIState.chasePlayer)
            {
                //going to last indicated point
                changeAIState(AIState.searchingPlayer);
                maxDistanceToStartChase -= reduceMinDistanceChaseValue;
            }
        }
    }
}