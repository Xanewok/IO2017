using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for items.
/// It is abstract, as it have no way to distinguish between different type of its.
/// </summary>
public abstract class SimpleItem : ItemObject {

    /// <summary>
    /// Relative position from player when equipped. For left hand, X Axis is negated.
    /// </summary>
    [Tooltip("Relative position from player when equipped. For left hand, X Axis is negated.")]
    public Vector3 equippedRel = new Vector3(0.5f, 0.0f, 0.5f);
    /// <summary>
    /// Relative rotation from player when equipped.
    /// </summary>
    [Tooltip("Relative rotation from player when equipped.")]
    public Vector3 equippedRot = Vector3.zero;
    /// <summary>
    /// Visible sprite of that item
    /// </summary>
    [Tooltip("Visible sprite of that item")]
    public Sprite image;

    private bool started = false;
    /// <summary>
    /// What slot is this item equipped
    /// </summary>
    protected int equippedSlot = -1;
    /// <summary>
    /// Rigidbody of an item
    /// </summary>
    protected Rigidbody rigid;
    /// <summary>
    /// Collider of an item
    /// </summary>
    protected Collider collid;
    /// <summary>
    /// MeshRenderer of that item
    /// </summary>
    protected MeshRenderer meshRenderer;

    void Start()
    {
        if (!started) onStart();
    }

    /// <summary>
    /// Method invoked on start for that item.
    /// </summary>
    protected virtual void onStart()
    {
        started = true;
        rigid = gameObject.GetComponent<Rigidbody>();
        collid = gameObject.GetComponent<Collider>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Checks if item is equipped
    /// </summary>
    /// <returns>True if equipped, false otherwise</returns>
    public bool isItemEquipped()
    {
        return equippedSlot != -1;
    }

    /// <summary>
    /// Remove object from equipment slot if equipped.
    /// </summary>
    /// <returns>True on succes, false otherwise</returns>
    protected bool removeObject()
    {
        if (isItemEquipped())
        {
            wearerInventory.setEquippedItem(equippedSlot, null);
            return true;
        }
        else
        {
            return false;
        }
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
        equippedSlot = hand;
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
        equippedSlot = -1;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        meshRenderer.enabled = false;
    }

    public override void onUseStart()
    {
    }

    public override void onUse()
    {
    }

    public override void onUseEnd()
    {
    }

    public override Sprite getSprite()
    {
        return image;
    }
}
