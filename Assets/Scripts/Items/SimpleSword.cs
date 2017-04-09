using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSword : SimpleItem {

    public Animator bladeAnimator;
    public SimpleBlade blade;

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
        blade.onPick();
    }

    public override void onEquip(int hand)
    {
        base.onEquip(hand);
        blade.onEquip();
    }

    public override void onUnEquip()
    {
        base.onUnEquip();
        blade.onUnEquip();
        this.onUseEnd();
    }

    public override void onUseStart()
    {
        bladeAnimator.SetBool("attacking", true);
    }

    public override void onUseEnd()
    {
        bladeAnimator.SetBool("attacking", false);
    }

}
