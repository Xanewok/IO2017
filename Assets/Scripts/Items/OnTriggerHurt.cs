using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerHurt : MonoBehaviour {

	public float damage = 10f;
	public int damageType = 0;
	[Tooltip("Leave empty to affect all objects")]
	public string targetTag;
	// TODO: This is a hack, we need to setup proper physical layers and collision matrix
	[Tooltip("Ignore objects with given tag")]
	public string ignoreTag;

	void OnTriggerEnter(Collider other) {
		if ((targetTag.Length != 0 && !other.CompareTag(targetTag)) ||
			(ignoreTag.Length != 0 && other.CompareTag(ignoreTag)))
			return;
		Status stat = other.gameObject.GetComponent<Status>();
		if (stat != null)
		{
			stat.hurt(damage, damageType);
		}
	}
}
