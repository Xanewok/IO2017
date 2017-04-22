using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StackableItem : SimpleItem {

    protected TextSpawner spawn;
    protected int quantity = 1;
    protected int equippedSlot = -1;

    protected override void onStart()
    {
        base.onStart();
        spawn = gameObject.GetComponent<TextSpawner>();
    }

    public int getQuantity()
    {
        return quantity;
    }

    public override void onEquip(int hand)
    {
        base.onEquip(hand);
        equippedSlot = hand;
    }

    public override void onUnEquip()
    {
        base.onUnEquip();
        equippedSlot = -1;
    }

    public override void onUseStart()
    {
        base.onUseStart();
        if (equippedSlot == Inventory.hand_right) onUseWithSecondSlot(Inventory.hand_left);
        else if (equippedSlot == Inventory.hand_left) onUseWithSecondSlot(Inventory.hand_right);
    }

    //For convinience Used and giving alternate slot
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

    /*
        Used to split item.
        It should instantiate new item, and make it child of this item's parent.
        It should also change quantity accordingly.
    */
    protected abstract StackableItem splitItem();
    /*
        Used to get name and quantity of item in a message.
        That message if existant, would there be displayed on screen.
    */
    protected abstract string getMessage();

    public virtual bool areItemsStackable(ItemObject item)
    {
        return item.GetType().Equals(this.GetType());
    }

    protected virtual void onUseWithSecondItem(ItemObject item)
    {
        if (areItemsStackable(item))
        {
            StackableItem it = (StackableItem) item;
            it.quantity += 1;
            this.quantity -= 1;
        }
    }

    // Checks object quantity, and destroys it if 0
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