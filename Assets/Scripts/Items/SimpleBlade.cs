using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBlade : MonoBehaviour {

    public float damage = 10f;
    public int damageType = 0;
    [Tooltip("For animator: should the blade damage objects.")]
    public bool damages = false;

    Collider collid;
    MeshRenderer meshRender;

    void Start()
    {
        collid = gameObject.GetComponent<Collider>();
        meshRender = gameObject.GetComponent<MeshRenderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Status stat = collision.gameObject.GetComponent<Status>();
        if (damages) print(collision.gameObject.name);
        if (stat != null && damages)
        {
            stat.hurt(damage, damageType);
        }
    }

    public void onEquip()
    {
        collid.enabled = true;
        meshRender.enabled = true;
    }

    public void onUnEquip()
    {
        collid.enabled = false;
        meshRender.enabled = false;
    }

    public void onPick()
    {
        collid.enabled = false;
        meshRender.enabled = false;
    }

    public void onDrop()
    {
        collid.enabled = false;
        meshRender.enabled = true;
    }
}
