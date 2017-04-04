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
    [Tooltip("Relative x change for left/right hand")]
    public float x_equipped_rel = 0.5f;
    public float y_equipped = 0.0f;
    public float z_equipped = 0.5f;
    public float x_equipped_rot = 10.0f;
    public float y_equipped_rot = 0.0f;
    public float z_equipped_rot = 0.0f;
    public Sprite image;
    public Light light;

    protected bool equipped = false;
    protected Rigidbody rigid;
    protected Collider collid;
    protected MeshRenderer meshRenderer;
    protected MeshRenderer headRenderer;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        collid = gameObject.GetComponent<Collider>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        headRenderer = gameObject.transform.GetComponentInChildren<MeshRenderer>();
    }

    public override void onDropDown(GameObject player, Inventory inventory)
    {
        base.onDropDown(player, inventory);
        collid.enabled = true;
        meshRenderer.enabled = true;
        headRenderer.enabled = true;
        light.enabled = true;
        rigid.isKinematic = false;
    }

    public override void onEquip(int hand)
    {
        equipped = true;
        float x_equipped = 0f;
        if (hand == 0)
            x_equipped = x_equipped_rel;
        else if (hand == 1)
            x_equipped = -x_equipped_rel;
        transform.localPosition = new Vector3(x_equipped, y_equipped, z_equipped);
        transform.localRotation = Quaternion.Euler(x_equipped_rot, y_equipped_rot, z_equipped_rot);
        meshRenderer.enabled = true;
        headRenderer.enabled = true;
        light.enabled = true;
    }

    public override void onPickUp(GameObject player, Inventory inventory)
    {
        base.onPickUp(player, inventory);
        collid.enabled = false;
        rigid.isKinematic = true;
        meshRenderer.enabled = false;
        headRenderer.enabled = false;
        light.enabled = false;
    }

    public override void onUnEquip()
    {
        equipped = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        meshRenderer.enabled = false;
        headRenderer.enabled = false;
        light.enabled = false;
    }

    public override void onUse()
    {
    }

    public override Sprite getSprite()
    {
        return image;
    }
}
