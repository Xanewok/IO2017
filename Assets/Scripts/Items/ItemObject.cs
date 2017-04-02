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
public abstract class ItemObject : MonoBehaviour {

    protected Rigidbody rigid;
    protected Collider collid;
    protected MeshRenderer renderer;
    protected float pickUpDelay = 0f;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        collid = gameObject.GetComponent<Collider>();
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (pickUpDelay > 0f)
        {
            pickUpDelay -= Time.deltaTime;
            if (pickUpDelay < 0f)
            {
                pickUpDelay = 0f;
            }
        }
    }

    public abstract void onUse();

    // Wywoływane przy podniesieniu obiektu.
    // player - Gracz podnoszący przedmiot
    // inventory - Skrypt ekwipunku
    public virtual void onPickUp(GameObject player, PlayerInventory inventory)
    {
        this.transform.parent = player.transform;
        collid.enabled = false;
        renderer.enabled = false;
    }

    // Wywoływane przez upuszczenie obiektu
    // inventory - Skrypt ekwipunku
    public virtual void onDropDown(GameObject player, Inventory inventory)
    {
        this.transform.parent = null;
        collid.enabled = true;
        renderer.enabled = true;
        this.pickUpDelay = 1f;
    }

    // Wywoływane przy wyekwipowaniu obiektu
    public abstract void onEquip(int hand);

    // Wywoływane przy zdjęciu przedmiotu
    public abstract void onUnEquip();

    public virtual bool canBePicked(GameObject player)
    {
        return pickUpDelay == 0f;
    }

    public virtual Sprite getSprite()
    {
        return null;
    }
}
