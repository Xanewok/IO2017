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
#if UNITY_EDITOR // Compilation warning, TODO: replace 'light' identifier and check if it breaks anything
    new public Light light;
#else
    public Light light;
#endif

    public MeshRenderer headRenderer;

    protected override void onStart()
    {
        base.onStart();
        //headRenderer = gameObject.transform.GetComponentInChildren<MeshRenderer>();
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
