using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCollectible : StackableItem {

    [Tooltip("Type. Items of different type do not stack.")]
    public int type = 0;
    [Tooltip("Name shown when changing/splitting items")]
    public String shownName= "coins";

    protected override StackableItem splitItem()
    {
        GameObject newItem = Instantiate(gameObject, transform.parent);
        SimpleCollectible col = newItem.GetComponent<SimpleCollectible>();
        col.quantity = 1;
        this.quantity--;
        return col;
    }

    public override bool areItemsStackable(ItemObject item)
    {
        if (base.areItemsStackable(item))
        {
            SimpleCollectible collItem = (SimpleCollectible)item;
            return collItem.type == this.type;
        }
        else return false;
    }

    protected override string getMessage()
    {
        return this.quantity.ToString() + " " + this.shownName;
    }
}
