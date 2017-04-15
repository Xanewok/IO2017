using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackableItem : SimpleItem {

    protected int quantity = 1;
    protected int equippedSlot = -1;

    public int getQuantity()
    {
        return quantity;
    }

    public override void onEquip(int hand)
    {
        base.onEquip(hand);
        equippedSlot = -1;
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
    public virtual void onUseWithSecondSlot(int slot)
    {
        Inventory inv = this.wearer.GetComponent<Inventory>();
        if (inv != null)
        {
            ItemObject obj = inv.getEquippedItem(slot);
            if (obj != null) onUseWithSecondItem(obj);
            else //Move 1 instance of object to free slot
            {

            }
            checkIfNotEmpty();
        }
    }

    public virtual void onUseWithSecondItem(ItemObject item)
    {
        if (item.GetType().Equals(this.GetType()))
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
    }
}