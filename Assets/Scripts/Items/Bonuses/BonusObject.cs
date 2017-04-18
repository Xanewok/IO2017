using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BonusObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected abstract void OnUse();

    void OnTriggerEnter(Collider other)
    {
        PlayerControlls player = other.gameObject.GetComponent<PlayerControlls>();
        if (player != null)
        {
            this.OnUse();
            Destroy(gameObject);
        }
    }
}
