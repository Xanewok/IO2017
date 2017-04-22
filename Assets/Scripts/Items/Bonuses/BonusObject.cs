using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Bonus superclass.
    Each instance is one bonus.
    It is there to give functional base for bonus.
*/
public abstract class BonusObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
        Function executed when bonus is used. (aka player touches it)
    */
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
