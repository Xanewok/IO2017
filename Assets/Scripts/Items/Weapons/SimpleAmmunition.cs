using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component used for every simple type of ammunition
/// </summary>
public class SimpleAmmunition : SimpleCollectible {
    protected override void onUseWithSecondItem(ItemObject item)
    {
        AmmoReloadInterface reloadable = (AmmoReloadInterface)item;
        if (reloadable != null && reloadable.canUseAmmunition(this))
        {
            quantity--;
            reloadable.addAmmunition(new AmmoType(type));
        } 
        else
        {
            base.onUseWithSecondItem(item);
        }
    }
}
