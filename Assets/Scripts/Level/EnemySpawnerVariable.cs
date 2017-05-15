using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerVariable : MonoBehaviour
{
    [Serializable]
    public struct PrefabSpawnThreshold
    {
        [SerializeField] public GameController.Difficulty difficulty;
        [SerializeField] public float minimumScore;
        [SerializeField] public GameObject prefab;
    }

    public PrefabSpawnThreshold[] prefabSpawns;
    [Tooltip("Allow to spawn prefabs from higher/lower thresholds up to a specified margin.")]
    public int scoreTolerance = 5;
	public float initialDelay = 1.0f;
	public float spawnDelay = 5.0f;
	private static System.Random rng = new System.Random();

    private void Awake()
    {
        prefabSpawns = prefabSpawns.OrderBy(kv=> kv.difficulty).ThenByDescending(kv => kv.minimumScore).ToArray();
    }

    public void spawn(GameObject prefab) {
		if (prefab != null)
			Instantiate(prefab, transform.position, transform.rotation);
	}

	IEnumerator SpawnCoroutine()
	{
		yield return new WaitForSeconds(initialDelay);
		while (true)
		{
            float scoreTest = GetCurrentScore() + (rng.Next(0, 2 * scoreTolerance) - scoreTolerance);
            var prefabs = prefabSpawns.Where(spawn => spawn.difficulty == GameController.Instance.difficulty);
            foreach (var kv in prefabs)
            {
                if (scoreTest >= kv.minimumScore)
                {
                    spawn(kv.prefab);
                    break;
                }
            }

			yield return new WaitForSeconds(spawnDelay);
		}
	}

	void OnEnable()
	{
		StartCoroutine(SpawnCoroutine());
	}

	void OnDisable()
	{
		StopAllCoroutines();
	}

    static float GetCurrentScore()
    {
        var scoredGameMode = GameController.Instance.gameMode as IScoredGameMode<Int32>;

        return scoredGameMode != null ? scoredGameMode.GetScore(null) : 0.0f;
    }
}
