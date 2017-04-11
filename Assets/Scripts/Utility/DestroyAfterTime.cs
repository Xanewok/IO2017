using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    [Tooltip("Timeout")]
    public float timeToDestroy = 1f;
	
	// Update is called once per frame
	void Update () {
        timeToDestroy = timeToDestroy - Time.deltaTime;
        if (timeToDestroy <= 0f)
            Destroy(gameObject);
	}
}
