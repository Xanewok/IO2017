using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Inventory class for player.
**/
public class PlayerInventory : Inventory {

    public float fadeInScale = 25f;
    public float fadeOutScale = 15f;
    [Tooltip("What is the minimal scale for inventory (when it's invisible")]
    public float inventoryScaleStart = 0.1f;
    [Tooltip("At what minimal scale should the inventory be visible")]
    public float inventoryScaleVisible = 0.2f;
    [Tooltip("What is the final scale of inventory (when it's fully open)")]
    public float inventoryScaleEnd = 1.0f;
    [Tooltip("What inventory slot should be used to drop items")]
    public int deleteSlot = 0;
    [Tooltip("What sprite should be used for empty inventory slots")]
    public Sprite emptySlotSprite;
    [Tooltip("What sprite should be used for drop inventory slots")]
    public Sprite deleteSlotSprite;

    [Tooltip("Optional: player script")]
    public PlayerControlls player;
    [Tooltip("Optional: canvas used for inventory")]
    public Canvas canvas;

    [Tooltip("Number of usable inventory slots (excluding one used to drop items).")]
    public static int inventorySlots = 7;
    [Tooltip("Number of usable equipped slots.")]
    public static int equippedSlots = 2;

    UnityEngine.UI.Image[] images = new UnityEngine.UI.Image[inventorySlots + 1];
    ItemObject[] equipped = new ItemObject[equippedSlots];
    ItemObject[] picked = new ItemObject[inventorySlots];
    int lastSelected = 0; //Last selected inventory slots

    // Use this for initialization
    void Start () {
        if (player == null)
        {
            player = gameObject.GetComponent<PlayerControlls>();
        }
        if (canvas == null)
        {
            canvas = gameObject.transform.GetComponentInChildren<Canvas>();
        }
        canvas.enabled = false;

        // TODO: Might need a better arrange.
        // Especially we might want to make inventory created dynamically.
        int i = 0;
        foreach (Transform child in canvas.transform)
        {
            images[i] = child.gameObject.GetComponent<UnityEngine.UI.Image>();
            i++;
        }
        images[deleteSlot].overrideSprite = deleteSlotSprite;

    }
	
    //Checks if inventory needs to be opened
    public bool isInventoryOpen()
    {
        return Input.GetButton("Inventory_" + player.getPlayerNum());
    }

    void ShowInventory()
    {
        canvas.scaleFactor = Mathf.Lerp(canvas.scaleFactor, inventoryScaleEnd, Time.deltaTime * fadeInScale);
        if (canvas.scaleFactor >= inventoryScaleVisible)
            canvas.enabled = true;

        Vector3 aim = player.transform.rotation.eulerAngles;
        float angle = aim.y;

        int kol = (int)(angle + 360f/(inventorySlots+1f)/2f);
        while (kol < 0) kol += 360;
        kol /= 360/(inventorySlots+1);
        kol %= inventorySlots + 1;

        images[getVisibleSelectedSlot()].color = Color.white;
        images[kol].color = Color.green;
        lastSelected = kol;
    }

    void HideInventory()
    {
        if (canvas.scaleFactor > inventoryScaleVisible)
        {
            canvas.scaleFactor = Mathf.Lerp(canvas.scaleFactor, inventoryScaleStart, Time.deltaTime * fadeOutScale);
        }
        else
        {
            canvas.enabled = false;
            canvas.scaleFactor = inventoryScaleStart;
        }
    }

    void ManageInventory()
    {
        if (lastSelected != -1)
        {
            if (Input.GetButtonDown("Use_r_" + player.getPlayerNum()))
            {
                ManageItem(Inventory.hand_right);
            }
            else if (Input.GetButtonDown("Use_l_" + player.getPlayerNum()))
            {
                ManageItem(Inventory.hand_left);
            }
        }
    }


    void ManageUsingItems()
    {
        if (equipped[Inventory.hand_right] != null)
        {
            if (Input.GetButtonDown("Use_r_" + player.getPlayerNum())) equipped[Inventory.hand_right].onUseStart();
            else if (Input.GetButton("Use_r_" + player.getPlayerNum())) equipped[Inventory.hand_right].onUse();
            else if (Input.GetButtonUp("Use_r_" + player.getPlayerNum())) equipped[Inventory.hand_right].onUseEnd();
        }
        if (equipped[Inventory.hand_left] != null)
        {
            if (Input.GetButtonDown("Use_l_" + player.getPlayerNum())) equipped[Inventory.hand_left].onUseStart();
            else if (Input.GetButton("Use_l_" + player.getPlayerNum())) equipped[Inventory.hand_left].onUse();
            else if (Input.GetButtonUp("Use_l_" + player.getPlayerNum())) equipped[Inventory.hand_left].onUseEnd();
        }
    }

    //Used to drop/swap item from equipped into inventory
    void ManageItem(int num)
    {
        if (getSelectedSlot() == -1)
        {
            if (equipped[num] != null)
                equipped[num].onDropDown(player.gameObject, this);
            equipped[num] = null;
        }
        else
        {
            if (equipped[num] != null)
                equipped[num].onUnEquip();
            if (picked[getSelectedSlot()] != null)
                picked[getSelectedSlot()].onEquip(num);
            ItemObject tmp = equipped[num];
            if (tmp != null && tmp.getSprite() != null)
                images[getVisibleSelectedSlot()].overrideSprite = tmp.getSprite();
            else
                images[getVisibleSelectedSlot()].overrideSprite = emptySlotSprite;
            equipped[num] = picked[getSelectedSlot()];
            picked[getSelectedSlot()] = tmp;
        }       
    }

    //Remainder - visible slot 0 is used to delete items
    public int getVisibleSelectedSlot()
    {
        return lastSelected;
    }

    public void setVisibleSelectedSlot(int selected)
    {
        lastSelected = selected;
    }

    //Returns selected slot;
    // -1 if it's item delete slot
    public int getSelectedSlot()
    {
        if (lastSelected == deleteSlot) return -1;
        return lastSelected > deleteSlot ? lastSelected - 1 : lastSelected;
    }

    void OnCollisionEnter(Collision collision)
    {
        ItemObject obj = collision.gameObject.GetComponent<ItemObject>();
        if (obj != null && obj.canBePicked(player.gameObject))
        {
            if (equipped[Inventory.hand_right] == null)
            {
                equipped[Inventory.hand_right] = obj;
                equipped[Inventory.hand_right].onPickUp(player.gameObject, this);
                equipped[Inventory.hand_right].onEquip(Inventory.hand_right);
            }
            else if (equipped[Inventory.hand_left] == null)
            {
                equipped[Inventory.hand_left] = obj;
                equipped[Inventory.hand_left].onPickUp(player.gameObject, this);
                equipped[Inventory.hand_left].onEquip(Inventory.hand_left);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //Inventory Visible
		if (isInventoryOpen())
        {
            ShowInventory();
            if (canvas.enabled)
                ManageInventory();
        }
        else
        //Inventory Invisible
        {
            HideInventory();
            ManageUsingItems();
        }
	}

    public override ItemObject getPickedItem(int slot)
    {
        if (slot < inventorySlots)
        {
            return this.picked[slot];
        }
        else
        {
            return null;
        }
    }

    public override ItemObject getEquippedItem(int slot)
    {
        if (slot < equippedSlots)
        {
            return this.equipped[slot];
        }
        else
        {
            return null;
        }
    }

    public override bool setPickedItem(int slot, ItemObject obj)
    {
        if (slot < inventorySlots)
        {
            picked[slot] = obj;
            return true;
        }
        else return false;
    }

    public override bool setEquippedItem(int slot, ItemObject obj)
    {
        if (slot < equippedSlots)
        {
            equipped[slot] = obj;
            return true;
        }
        else return false;
    }
}
