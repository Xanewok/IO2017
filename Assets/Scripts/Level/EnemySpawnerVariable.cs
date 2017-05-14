using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerVariable : MonoBehaviour {
	[Tooltip("List of prefabs (null for not generating)")]
	public GameObject[] prefabs;
	[Tooltip("List of minimum score for that prefab to be generated.")]
	public float[] variability;
	public int scoreCap = 400;
	public int randomMax = 10;
	public float initialDelay = 1.0f;
	public float spawnDelay = 5.0f;
	private static System.Random rng = new System.Random();

	public void spawn(GameObject prefab) {
		if (prefab != null)
			Instantiate(prefab, transform.position, transform.rotation);
	}

	IEnumerator SpawnCoroutine()
	{
		yield return new WaitForSeconds(initialDelay);
		while (true)
		{
			bool generated = false;
			int randomInt = rng.Next (0, randomMax) + Mathf.Min(scoreCap, 0); //TODO: Dopisać score'a
			for (int x = 0; x < variability.Length; x++) {
				if (variability [x] >= randomInt) {
					generated = true;
					spawn (prefabs [x]);
					break;
				}
			}
			if (!generated) {
				spawn (prefabs [variability.Length - 1]);
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
}
