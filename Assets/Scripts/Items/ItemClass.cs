using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nadklasa dla przedmiotów.
    Nie używać jako komponentu, to NIE jest komponent.
    Nie dodawać mu MonoBehaviour - jest usuwany przy destroy.
    Pojedyncza instancja jest przekazywana jako obiekt (jest z nim równoważna) w porównaniu do komponentów
    które nie zachowują tych własności
*/
public abstract class ItemClass {

    public abstract void onUse();

    // Wywoływane przy podniesieniu obiektu.
    // player - Gracz podnoszący przedmiot
    // inventory - Skrypt ekwipunku
    public abstract void onPickUp(GameObject player, PlayerInventory inventory);

    // Wywoływane przez upuszczenie obiektu
    // inventory - Skrypt ekwipunku
    public abstract void onDropDown(GameObject player, Inventory inventory);

    // Wywoływane przy wyekwipowaniu obiektu
    // Obiekt sam powinien się zatroszczyć o dodanie się do gracza.
    public abstract void onEquip(int hand);

    // Wywoływane przy zdjęciu przedmiotu
    // Obiekt powinien się usunąć
    public abstract void onUnEquip();

    //Wywoływane na updacie:w
    public abstract void onUpdate(GameObject obj);

    public abstract bool canBePicked(GameObject player);

    public virtual Sprite getSprite()
    {
        return null;
    }
}
