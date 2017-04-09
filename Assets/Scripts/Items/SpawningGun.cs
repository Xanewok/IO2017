using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningGun : SimpleItem {

    public GameObject bullet;
    public float cooldownTime = 1f;
    public float startingSpeed = 1f;
    protected float cooldown = 0f;

    void Update()
    {
        cooldown = Mathf.Max(0f, cooldown - Time.deltaTime);
    }

    public override void onUse()
    {
        if (Mathf.Approximately(cooldown, 0f))
        {
            Rigidbody wearers = wearer.GetComponent<Rigidbody>();
            Vector3 addPos = Vector3.zero;
            if (wearers != null) addPos = wearers.velocity * Time.deltaTime;
            GameObject bull = Instantiate(bullet, transform.position + addPos, transform.rotation);
            Rigidbody rigid = bull.GetComponent<Rigidbody>();
            rigid.velocity = transform.rotation * Vector3.forward * startingSpeed;
        }
    }

}
