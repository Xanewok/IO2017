using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDead : MonoBehaviour {

	public GameObject[] spawnable;

	void Start () {
		gameObject.GetComponent<Status>().healthChanged += OnEnemyHealthChanged;
	}

	void OnEnemyHealthChanged(GameObject enemy, float health)
	{
		if (health <= 0) {
			int i = Random.range (0, spawnable.Length ());
			if (spawnable [i] != null) {
				Instantiate (spawnable [i], transform.position);
			}
		}
	}
}
