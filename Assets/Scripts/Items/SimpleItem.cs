using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleItem : ItemObject {

    [Tooltip("Relative position from player when equipped. For left hand, X Axis is negated.")]
    public Vector3 equippedRel = new Vector3(0.5f, 0.0f, 0.5f);
    [Tooltip("Relative rotation from player when equipped.")]
    public Vector3 equippedRot = Vector3.zero;
    public Sprite image;

    protected bool equipped = false;
    protected Rigidbody rigid;
    protected Collider collid;
    protected MeshRenderer meshRenderer;

    void Start()
    {
        onStart();
    }

    protected virtual void onStart()
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
        rigid.isKinematic = false;
    }

    public override void onEquip(int hand)
    {
        equipped = true;
        float x_equipped = 0f;
        if (hand == 0)
            x_equipped = equippedRel.x;
        else if (hand == 1)
            x_equipped = -equippedRel.x;
        transform.localPosition = new Vector3(x_equipped, equippedRel.y, equippedRel.z);
        transform.localRotation = Quaternion.Euler(equippedRot);
        meshRenderer.enabled = true;
    }

    public override void onPickUp(GameObject player, Inventory inventory)
    {
        base.onPickUp(player, inventory);
        collid.enabled = false;
        rigid.isKinematic = true;
        meshRenderer.enabled = false;
    }

    public override void onUnEquip()
    {
        equipped = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        meshRenderer.enabled = false;
    }

    public override void onUse()
    {
    }

    public override Sprite getSprite()
    {
        return image;
    }
}
