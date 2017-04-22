using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory superclass. (generally interface).
/// Everythin that is capable of using items should have instance of such object.
/// </summary>
public abstract class Inventory : MonoBehaviour {
    /// <summary>
    /// Constant value for right hand slot.
    /// </summary>
    public const int hand_right = 0;
    /// <summary>
    /// Constant value for right hand slot.
    /// </summary>
    public const int hand_left = 1;

    /// <summary>
    /// Used to get information about objects in inventory.
    /// </summary>
    /// <returns>Item in that slot, or null if no item, or slot nonexistant</returns>
    public abstract ItemObject getPickedItem(int slot);
    /// <summary>
    /// Used to set object in inventory.
    /// Mostly unsafe as it doesn't execute any of item's methods.
    /// </summary>
    /// <param name="slot">slot which we want to set this item for.</param>
    /// <param name="obj">object which we want to set</param>
    /// <returns>true on success, false on failure</returns>
    public virtual bool setPickedItem(int slot, ItemObject obj)
    {
        return false;
    }

    /// <summary>
    /// Used to get information about equipped objects.
    /// </summary>
    /// <param name="slot">Number of slot we want to get item from</param>
    /// <returns>Item in that slot, or null if no item, or slot nonexistant</returns>
    public abstract ItemObject getEquippedItem(int slot);

    /// <summary>
    /// Used to set equipped objects.
    /// Mostly unsafe as it doesn't execute any of item's methods.
    /// </summary>
    /// <param name="slot">slot which we want to set this item for.</param>
    /// <param name="obj">object which we want to set</param>
    /// <returns>true on success, false on failure</returns>
    public virtual bool setEquippedItem(int slot, ItemObject obj)
    {
        return false;
    }

}
