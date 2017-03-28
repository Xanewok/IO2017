using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    // Wywoływane przy podniesieniu obiektu.
    // player - Gracz podnoszący przedmiot
    // inventory - Skrypt ekwipunku
    public void onPickUp(GameObject player, PlayerInventory inventory)
    {
    }

    // Wywoływane przez upuszczenie obiektu
    // inventory - Skrypt ekwipunku
    public void onDropDown(GameObject player, Inventory inventory)
    {

    }

    // Wywoływane przy wyekwipowaniu obiektu
    // Obiekt sam powinien się zatroszczyć o dodanie się do gracza.
    public void onEquip(int hand)
    {

    }

    // Wywoływane przy zdjęciu przedmiotu
    // Obiekt powinien się usunąć
    public void onUnEquip()
    {

    }

    public Sprite getSprite()
    {
        return null;
    }
}
