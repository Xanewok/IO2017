using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour {
    public const int hand_right = 0;
    public const int hand_left = 1;

    /**
        Used to get information about objects in inventory.
        @return Item in that slot, or null if no item, or slot nonexistant
    */
    public abstract ItemObject getPickedItem(int slot);
    /**
        Used to set object in inventory.
        Mostly unsafe as it doesn't execute any of item's methods.
        @param slot slot which we want to set this item for.
        @param obj object which we want to set
        @return true on success, false on failure
    */
    public virtual bool setPickedItem(int slot, ItemObject obj)
    {
        return false;
    }

    /**
        Used to get information about equipped objects.
        @return Item in that slot, or null if no item, or slot nonexistant
    */
    public abstract ItemObject getEquippedItem(int slot);

    /**
        Used to set equipped objects.
        Mostly unsafe as it doesn't execute any of item's methods.
        @param slot slot which we want to set this item for.
        @param obj object which we want to set
        @return true on success, false on failure
    */
    public virtual bool setEquippedItem(int slot, ItemObject obj)
    {
        return false;
    }

}
