using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for rapier type item.
/// -Forward attacking dash
/// </summary>
public class RapierSword : SimpleItem {

    /// <summary>
    /// Animator of blade
    /// </summary>
    public Animator bladeAnimator;
    /// <summary>
    /// Blade script
    /// </summary>
    public RapierBlade blade;

    protected override void onStart()
    {
        base.onStart();
        meshRenderer = blade.gameObject.GetComponent<MeshRenderer>();
    }

    public override void onDropDown(GameObject player, Inventory inventory)
    {
        base.onDropDown(player, inventory);
        blade.onDrop();
    }

    public override void onPickUp(GameObject player, Inventory inventory)
    {
        base.onPickUp(player, inventory);
        blade.onPick(player);
    }

    public override void onEquip(int hand)
    {
        base.onEquip(hand);
        blade.onEquip();
        bladeAnimator.SetTrigger("UnEquip");
    }

    public override void onUnEquip()
    {
        base.onUnEquip();
        blade.onUnEquip();
        bladeAnimator.SetTrigger("UnEquip");
    }

    public override void onUseStart()
    {
            bladeAnimator.SetTrigger("AttackStart");
    }

    public override void onUseEnd()
    {
        bladeAnimator.SetTrigger("AttackEnd");
    }
}
