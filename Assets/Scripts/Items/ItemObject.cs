using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Item superclass.
    Each instance is one object. (though this logic may be changed for some items)
    Instance shouldn't be deleted when it is in inventory.
*/
public abstract class ItemObject : MonoBehaviour {

    [Tooltip("For how long item should be unpickable after being dropped.")]
    public float delayOnDrop = 1f;

    protected GameObject wearer;
    protected float pickUpDelay = 0f;

    void Update()
    {
        onUpdate();
    }

    protected virtual void onUpdate()
    {
        if (pickUpDelay > 0f)
        {
            pickUpDelay = Mathf.Max(0f, pickUpDelay - Time.deltaTime);
        }
    }

    public abstract void onUseStart();
    public abstract void onUse();
    public abstract void onUseEnd();

    public virtual void onPickUp(GameObject player, Inventory inventory)
    {
        this.transform.SetParent(player.transform);
        wearer = player;
    }

    public virtual void onDropDown(GameObject player, Inventory inventory)
    {
        this.transform.SetParent(null);
        wearer = null;
        this.pickUpDelay = delayOnDrop;
    }

    public abstract void onEquip(int hand);

    public abstract void onUnEquip();

    public virtual bool canBePicked(GameObject player)
    {
        return Mathf.Approximately(pickUpDelay, 0f) && wearer == null;
    }

    public abstract Sprite getSprite();
}
