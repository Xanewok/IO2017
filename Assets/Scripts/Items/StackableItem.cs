using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for every stackable item.
/// </summary>
public abstract class StackableItem : SimpleItem {

    /// <summary>
    /// Text spawner used to show text
    /// </summary>
    protected TextSpawner spawn;
    /// <summary>
    /// Quantity of an object
    /// </summary>
    protected int quantity = 1;

    protected override void onStart()
    {
        base.onStart();
        spawn = gameObject.GetComponent<TextSpawner>();
    }

    /// <summary>
    /// Returns quantity of an object
    /// </summary>
    /// <returns>quantity</returns>
    public int getQuantity()
    {
        return quantity;
    }

    public override void onEquip(int hand)
    {
        base.onEquip(hand);
    }

    public override void onUnEquip()
    {
        base.onUnEquip();
    }

    public override void onUseStart()
    {
        base.onUseStart();
        if (equippedSlot == Inventory.hand_right) onUseWithSecondSlot(Inventory.hand_left);
        else if (equippedSlot == Inventory.hand_left) onUseWithSecondSlot(Inventory.hand_right);
    }

    /// <summary>
    /// For convinience Used and giving alternate slot
    /// </summary>
    /// <param name="slot">second slot</param>
    protected virtual void onUseWithSecondSlot(int slot)
    {
        if (wearerInventory != null)
        {
            ItemObject obj = wearerInventory.getEquippedItem(slot);
            if (obj != null) onUseWithSecondItem(obj);
            else //Move 1 instance of object to free slot
            {
                StackableItem split = this.splitItem();
                split.onStart();
                // To let item think that it is unequipped
                split.onUnEquip();
                split.onDropDown(wearer, wearerInventory);
                // Equipping it on free slot
                split.onPickUp(wearer, wearerInventory);
                split.onEquip(slot);
                wearerInventory.setEquippedItem(slot, split);
            }
            checkIfNotEmpty();
        }
    }

    /// <summary>
    /// Used to split item.
    /// It should instantiate new item, and make it child of this item's parent.
    /// It should also change quantity accordingly.
    /// </summary>
    /// <returns>New instance of that object, splitted accordingly</returns>
    protected abstract StackableItem splitItem();
    /// <summary>
    /// Used to get name and quantity of item in a message.
    /// That message if existant, would there be displayed on screen.
    /// </summary>
    /// <returns>Visible message</returns>
    protected abstract string getMessage();

    /// <summary>
    /// Function used to check if given items are stackable
    /// </summary>
    /// <param name="item">second item</param>
    /// <returns>true if stackable, false otherwise</returns>
    public virtual bool areItemsStackable(ItemObject item)
    {
        return item.GetType().Equals(this.GetType());
    }

    /// <summary>
    /// Triggered when we use stackable on different item
    /// </summary>
    /// <param name="item">item we use stackable on</param>
    protected virtual void onUseWithSecondItem(ItemObject item)
    {
        if (areItemsStackable(item))
        {
            StackableItem it = (StackableItem) item;
            it.quantity += 1;
            this.quantity -= 1;
        }
    }

    /// <summary>
    /// Checks object quantity, and destroys it if 0
    /// </summary>
    public virtual void checkIfNotEmpty()
    {
        if (quantity <= 0)
        {
            Inventory inv = this.wearer.GetComponent<Inventory>();
            inv.setEquippedItem(equippedSlot, null);    //Kinda unsafe. We're assuming, we can do this
            Destroy(gameObject);
        }
        else if (spawn != null)
        {
            string message = this.getMessage();
            if (message != null)
            {
                spawn.spawnText(message);
            }
        }
    }
}