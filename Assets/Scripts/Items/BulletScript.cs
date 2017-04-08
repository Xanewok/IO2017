using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float damage = 10f;
    public int damageType = 0;

	// Use this for initialization
	void Start () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Status stat = collision.gameObject.GetComponent<Status>();
        if (stat != null)
        {
            stat.hurt(damage, damageType);
        }
    }


}
