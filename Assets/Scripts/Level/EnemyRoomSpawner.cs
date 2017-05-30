using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using YAGTSS.Utility.Extensions;

public class EnemyRoomSpawner : MonoBehaviour
{
	[System.Serializable]
    public struct PrefabSpawnThreshold
    {
        public GameController.Difficulty difficulty;
        public float minimumScore;
        public GameObject prefab;
    }

	public PrefabSpawnThreshold[] prefabSpawns;

	public int minSpawnCount = 1;
	public int maxSpawnCount = 3;
	private int spawnCount;

	public BoxCollider[] floorColliders;

	private bool spawned = false;

	void Awake()
	{
		prefabSpawns = prefabSpawns.OrderBy(kv=> kv.difficulty).ThenByDescending(kv => kv.minimumScore).ToArray();
		spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
	}
    void Start()
    {
		StartCoroutine(TrySpawnCoroutine());
    }

	IEnumerator TrySpawnCoroutine()
	{
		while (!spawned)
		{
			yield return new WaitForSeconds(0.3f);

			TrySpawn();
			spawned = true;
		}
	}

	void TrySpawn()
	{
		int spawnedCount = 0;
		while (spawnedCount < spawnCount)
		{
			Vector3 source = RandomPointOnFloor();
			float maxDist = 2.0f;

			int attemptCount = 0;
			const int maxAttempts = 5;
			NavMeshHit hit = new NavMeshHit();
			for (; attemptCount < maxAttempts; ++attemptCount)
			{
				if (!NavMesh.SamplePosition(source, out hit, maxDist, NavMesh.AllAreas))
				{
					source = RandomPointOnFloor();
					maxDist += 1.0f;
					continue;
				}
				else
					break;
			}

			Debug.Assert(attemptCount < maxAttempts, "Could not find fitting position to spawn enemy");

			var enemy = RandomEnemy();
			Quaternion rotation = Random.rotation;
			Instantiate(enemy, hit.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
			spawnedCount++;
		}
	}

	Vector3 RandomPointOnFloor()
	{
		var box = floorColliders[Random.Range(0, floorColliders.Length)];

		// Randomize point on XZ plane, pick top Y coordinate
		return new Vector3 {
			x = box.bounds.center.x + box.bounds.extents.x * Random.Range(-1.0f, 1.0f),
			y = box.bounds.center.y + box.bounds.extents.y,
			z = box.bounds.center.z + box.bounds.extents.z * Random.Range(-1.0f, 1.0f),
		};
	}

	GameObject RandomEnemy()
	{
		var score = GetCurrentScore();

		var prefabs = prefabSpawns
			.Where(spawn => spawn.difficulty == GameController.Instance.difficulty)
			.Where(spawn => score >= spawn.minimumScore)
			.Shuffle()
			.ToArray();

		return prefabs[Random.Range(0, prefabs.Length)].prefab;
	}

	static float GetCurrentScore()
    {
        var scoredGameMode = GameController.Instance.gameMode as IScoredGameMode<System.Int32>;

        return scoredGameMode != null ? scoredGameMode.GetScore(null) : 0.0f;
    }
}
