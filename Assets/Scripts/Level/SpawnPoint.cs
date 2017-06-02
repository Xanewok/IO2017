using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YAGTSS.Level
{
    public class SpawnPoint : MonoBehaviour
    {
        public GameObject playerPrefab;
        public float initialSpawnDistance = 0.5f;

        void Awake()
        {
            SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
            if (spawnPoints.Length > 1)
            {
                Debug.LogError("There are more than 1 active objects of type SpawnPoint!");
            }

            SpawnPlayers();
        }

        void Start()
        {
            // Beam camera so we don't have to wait for it to follow
            StartCoroutine(delayedPlayerBeam());
        }

        void SpawnPlayers()
        {
            var count = GameController.Instance.gameMode.GetInitialPlayerCount();
            Debug.Log("Player count: " + count);
            List<Vector3> spawnedPositions = new List<Vector3>();
            const float playerRadius = 0.5f;

            for (int i = 0; i < count; ++i)
            {
                Vector3 pos = Vector3.zero;
                float spawnDist = initialSpawnDistance;
                for (int attempts = 0; attempts < 5; ++i)
                {
                    spawnDist = pos == Vector3.zero ? initialSpawnDistance : spawnDist + 0.1f;

                    pos = transform.position + Random.insideUnitSphere * spawnDist;
                    pos.y = transform.position.y + 2 * playerRadius;

                    Vector3 collisionPos = pos;
                    collisionPos.y += playerRadius;
                    if (!spawnedPositions.Any(spawn => (spawn - pos).magnitude < spawnDist) && !Physics.CheckSphere(pos, playerRadius))
                        break;
                }
                
                spawnedPositions.Add(pos);
                var player = Instantiate(playerPrefab, pos, Quaternion.Euler(Random.Range(0, 360), 0, Random.Range(0, 360)));
                player.GetComponent<PlayerControlls>().playerNum = i;
            }
        }

        IEnumerator delayedPlayerBeam()
        {
            yield return new WaitForEndOfFrame();

            GameObject player = GameObject.FindGameObjectsWithTag("Player").First();

            Vector3 camPos = transform.position;
            camPos.y = Camera.current.transform.position.y;
            Camera.current.transform.position = camPos;

            Vector3 spawnPos = transform.position;
            spawnPos.y = player.transform.position.y;
            player.transform.position = spawnPos;
        }
    }
}
