using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Item superclass.
    Each instance is one object. (though this logic may be changed for some items)
    Instance shouldn't be deleted when it is in inventory.
*/
public abstract class ItemObject : MonoBehaviour {

    /// <summary>
    /// How long item should be unavailable for picking after dripping it.
    /// </summary>
    [Tooltip("For how long item should be unpickable after being dropped.")]
    public float delayOnDrop = 1f;

    /// <summary>
    /// Object that wears this item. (or null if on ground)
    /// </summary>
    protected GameObject wearer;
    /// <summary>
    /// Inventory that has this item. (or null, if on ground)
    /// </summary>
    protected Inventory wearerInventory;
    /// <summary>
    /// Delay before item can be picked up again.
    /// </summary>
    protected float pickUpDelay = 0f;

    void Update()
    {
        onUpdate();
    }

    /// <summary>
    /// Event executed on every update.
    /// </summary>
    protected virtual void onUpdate()
    {
        if (pickUpDelay > 0f)
        {
            pickUpDelay = Mathf.Max(0f, pickUpDelay - Time.deltaTime);
        }
    }

    /// <summary>
    /// Triggered when we start using object
    /// </summary>
    public abstract void onUseStart();
    /// <summary>
    /// Triggered on prolonged use of object.
    /// </summary>
    public abstract void onUse();
    /// <summary>
    /// Triggered when we stop using object.
    /// (might not always be triggered).
    /// </summary>
    public abstract void onUseEnd();

    /// <summary>
    /// Triggered when item is picked up
    /// </summary>
    /// <param name="player">object that picks item</param>
    /// <param name="inventory">iventory that picks item</param>
    public virtual void onPickUp(GameObject player, Inventory inventory)
    {
        this.transform.SetParent(player.transform);
        wearer = player;
        wearerInventory = inventory;
    }

    /// <summary>
    /// Triggered when item is dropped
    /// </summary>
    /// <param name="player">object that drops item</param>
    /// <param name="inventory">inventory that drops item</param>
    public virtual void onDropDown(GameObject player, Inventory inventory)
    {
        this.transform.SetParent(null);
        wearer = null;
        wearerInventory = null;
        this.pickUpDelay = delayOnDrop;
    }

    /// <summary>
    /// Triggered when item is being equipped
    /// </summary>
    /// <param name="hand">equip slot on which that item is equipped</param>
    public abstract void onEquip(int hand);

    /// <summary>
    /// Triggered when item is being unequipped
    /// </summary>
    public abstract void onUnEquip();

    /// <summary>
    /// Used to check if item can be picked up.
    /// Used mainly to take care of delay before picking object up.
    /// </summary>
    /// <param name="player">object that want to pick item</param>
    /// <returns>true if can be picked, false otherwise</returns>
    public virtual bool canBePicked(GameObject player)
    {
        return Mathf.Approximately(pickUpDelay, 0f) && wearer == null;
    }

    /// <summary>
    /// Used to get sprite of object
    /// </summary>
    /// <returns>correct sprite or null if nonexistant</returns>
    public abstract Sprite getSprite();
}
