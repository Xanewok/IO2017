using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Testowy skrypt
    Do przełączania itemów
*/
public class LampScript : ItemObject
{

    public Sprite image;

    GameObject player;

    public override void onDropDown(GameObject player, Inventory inventory)
    {
        base.onDropDown(player, inventory);
    }

    public override void onEquip(int hand)
    {
    }

    public override void onPickUp(GameObject player, PlayerInventory inventory)
    {
        base.onPickUp(player, inventory);
        player.name = "Picked";
        this.player = player;
    }

    public override void onUnEquip()
    {

    }

    public override void onUse()
    {
    }

    public override Sprite getSprite()
    {
        return image;
    }
}
