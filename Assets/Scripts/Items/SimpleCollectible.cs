using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic class for all collectible stackable items.
/// </summary>
public class SimpleCollectible : StackableItem {

    /// <summary>
    /// Type of collectible.
    /// As a general rule, it is used to distinguish between different collectible types.
    /// Items with different types will not stack.
    /// </summary>
    [Tooltip("Type. Items of different type do not stack.")]
    public int type = 0;
    /*
        Consts for specific types of objects
    */
    /// <summary>
    /// Coins
    /// </summary>
    public const int typeCoins = 0;
    /// <summary>
    /// Shotty shells
    /// </summary>
    public const int typeShotgunShells = 1;

    /// <summary>
    /// Name that will be shown, when item is managed.
    /// </summary>
    [Tooltip("Name shown when changing/splitting items")]
    public String shownName= "coins";

    /// <summary>
    /// Used to split item.
    /// New instance should have the same parent. Sum of quantities shouldnt change.
    /// </summary>
    /// <returns>One piece of this item as the new instance.</returns>
    protected override StackableItem splitItem()
    {
        GameObject newItem = Instantiate(gameObject, transform.parent);
        SimpleCollectible col = newItem.GetComponent<SimpleCollectible>();
        col.quantity = 1;
        this.quantity--;
        return col;
    }

    /// <summary>
    /// Used to check if two types of items are stackable.
    /// It takes type into account
    /// </summary>
    /// <param name="item">Item we want to stack with ourselves.</param>
    /// <returns>true if possible, false otherwise.</returns>
    public override bool areItemsStackable(ItemObject item)
    {
        if (base.areItemsStackable(item))
        {
            SimpleCollectible collItem = (SimpleCollectible)item;
            return collItem.type == this.type;
        }
        else return false;
    }

    /// <summary>
    /// What message should be shown when item is managed
    /// </summary>
    /// <returns>visible message</returns>
    protected override string getMessage()
    {
        return this.quantity.ToString() + " " + this.shownName;
    }
}
