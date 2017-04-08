using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Item mainly for testing
    Will be a normal lamp/flashlight item, soon.
*/
public class LampScript : SimpleItem
{
    public Light light;
    protected MeshRenderer headRenderer;

    protected override void onStart()
    {
        base.onStart();
        headRenderer = gameObject.transform.GetComponentInChildren<MeshRenderer>();
    }

    public override void onDropDown(GameObject player, Inventory inventory)
    {
        base.onDropDown(player, inventory);
        headRenderer.enabled = true;
        light.enabled = true;
    }

    public override void onEquip(int hand)
    {
        base.onEquip(hand);
        headRenderer.enabled = true;
        light.enabled = true;
    }

    public override void onPickUp(GameObject player, Inventory inventory)
    {
        base.onPickUp(player, inventory);
        headRenderer.enabled = false;
        light.enabled = false;
    }

    public override void onUnEquip()
    {
        base.onUnEquip();
        headRenderer.enabled = false;
        light.enabled = false;
    }
}
