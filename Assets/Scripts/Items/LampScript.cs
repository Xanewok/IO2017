using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Item mainly for testing
    Will be a normal lamp/flashlight item, soon.
*/
public class LampScript : ItemObject
{

    public Sprite image;

    protected Rigidbody rigid;
    protected Collider collid;
    protected MeshRenderer meshRenderer;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        collid = gameObject.GetComponent<Collider>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public override void onDropDown(GameObject player, Inventory inventory)
    {
        base.onDropDown(player, inventory);
        collid.enabled = true;
        meshRenderer.enabled = true;
    }

    public override void onEquip(int hand)
    {
    }

    public override void onPickUp(GameObject player, Inventory inventory)
    {
        base.onPickUp(player, inventory);
        collid.enabled = false;
        meshRenderer.enabled = false;
    }

    public override void onUnEquip()
    {

    }

    public override void onUse()
    {
    }

    public override Sprite getSprite()
    {
        return image;
    }
}
