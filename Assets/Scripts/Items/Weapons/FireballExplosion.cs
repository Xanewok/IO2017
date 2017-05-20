using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour {

	public float damage = 10f;
	public GameObject[] instantiateOnDeath;
	public int damageType = 0;
	[Tooltip("Leave empty to affect all objects")]
	public string targetTag;
	// TODO: This is a hack, we need to setup proper physical layers and collision matrix
	[Tooltip("Ignore objects with given tag")]
	public string ignoreTag;

	void OnCollisionEnter(Collision collision)
	{
		if ((ignoreTag.Length != 0 && collision.collider.CompareTag (ignoreTag))) {
			return;
		}
		if ((targetTag.Length != 0 && !collision.collider.CompareTag (targetTag)) ||
			(ignoreTag.Length != 0 && collision.collider.CompareTag (ignoreTag))) {
			Destroy (gameObject); // To get rid of millions of bullets hanging on enemy's back and walls
			return;
		}
		
		foreach (GameObject prefab in instantiateOnDeath)
				Instantiate (prefab, transform.position, transform.rotation);
		Destroy(gameObject);
		Status stat = collision.gameObject.GetComponent<Status>();
		if (stat != null)
		{
			stat.hurt(damage, damageType);
		}
	}
}
