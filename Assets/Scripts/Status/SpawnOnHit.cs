using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnHit : MonoBehaviour {

	public GameObject onDead;
	public GameObject onHit;

	void Start () {
		gameObject.GetComponent<Status>().healthChanged += OnEnemyHealthChanged;
	}

	void OnEnemyHealthChanged(GameObject enemy, float health)
	{
		if (health <= 0 && onDead != null) {
			Instantiate (onDead, transform.position, transform.rotation);            
		} else if (onHit != null) {
			Instantiate (onHit, transform.position, transform.rotation);            
		}
	}
}
