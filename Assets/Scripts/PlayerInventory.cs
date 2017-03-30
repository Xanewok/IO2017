using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Klasa ekwipunku dla gracza.
    Umożliwia używanie przedmiotów.
**/
public class PlayerInventory : Inventory {

    public float fadeInScale = 25f;
    public float fadeOutScale = 15f;
    public Sprite emptySlotSprite;
    public Canvas canvas;
    public PlayerControlls player;
    UnityEngine.UI.Image[] images;
    int lastSelected = 0;
    ItemClass[] equipped;
    ItemClass[] picked;

	// Use this for initialization
	void Start () {
        images = new UnityEngine.UI.Image[8];
        if (player == null)
        {
            player = gameObject.GetComponent<PlayerControlls>();
        }
        if (canvas == null)
        {
            canvas = gameObject.transform.GetComponentInChildren<Canvas>();
        }
        equipped = new ItemClass[2];
        /*for(int i=0; i<2; i++)
        {
            equipped[i] = null;
        }*/
        picked = new ItemClass[7];
        /*for (int i = 0; i < 7; i++)
        {
            picked[i] = null;
        }*/
        canvas.enabled = false;
        {
            int i = 0;
            foreach (Transform child in canvas.transform)
            {
                images[i] = child.gameObject.GetComponent<UnityEngine.UI.Image>();
                i++;
            }
        }
	}
	
    public bool isInventoryOpen()
    {
        return Input.GetButton("Inventory_" + player.getPlayerNum());
    }

    void ShowInventory()
    {
        canvas.scaleFactor = Mathf.Lerp(canvas.scaleFactor, 1.0f, Time.deltaTime * fadeInScale);
        canvas.enabled = true;
        Vector3 aim = player.getLastTurn();
        Vector2 aim2d = Vector2.zero;
        aim2d.Set(aim.x, aim.z);
        if (aim2d.SqrMagnitude() > 0.5)
        {
            float angle = Vector2.Angle(Vector2.down, aim2d);
            if (Vector2.Angle(Vector2.left, aim2d) <= 90f)
            {
                angle = -angle;
            }
            int kol = (int)(angle + 22.5f);
            if (kol < 0) kol += 360;
            kol /= 45;
            if (lastSelected != -1)
                images[lastSelected].color = Color.white;
            images[kol].color = Color.green;
            lastSelected = kol;
        }
        else
        {
            if (lastSelected != -1)
                images[lastSelected].color = Color.white;
            lastSelected = -1;
        }
    }

    void HideInventory()
    {
        if (canvas.scaleFactor > 0.2)
        {
            canvas.scaleFactor = Mathf.Lerp(canvas.scaleFactor, 0.1f, Time.deltaTime * fadeOutScale);
        }
        else
        {
            canvas.enabled = false;
            canvas.scaleFactor = 0.1f;
        }
    }

    //Zarządzanie ekwipunkiem
    void ManageInventory()
    {
        if (lastSelected != -1)
        {
            if (Input.GetButtonDown("Use_r_" + player.getPlayerNum()))
            {
                ManageItem(0);
            }
            else if (Input.GetButtonDown("Use_l_" + player.getPlayerNum()))
            {
                ManageItem(1);
            }
        }
    }

    void ManageUsingItems()
    {
        if (Input.GetButtonDown("Use_r_" + player.getPlayerNum()) && equipped[0] != null)
        {
            equipped[0].onUse();
        }
        if (Input.GetButtonDown("Use_l_" + player.getPlayerNum()) && equipped[1] != null)
        {
            equipped[1].onUse();
        }
    }

    //Zarządzanie konkretnym przedmiotem
    void ManageItem(int num)
    {
        if (getSelectedSlot() == 0)
        {
            //print("Dropping Item\n");
            if (equipped[num] != null)
                equipped[num].onDropDown(player.gameObject, this);
            equipped[num] = null;
        }
        else
        {
            //print("Swapping Item\n");
            if (equipped[num] != null)
                equipped[num].onUnEquip();
            if (picked[getSelectedSlot() - 1] != null)
                picked[getSelectedSlot() - 1].onEquip(num);
            ItemClass tmp = equipped[num];
            //print("Uneqipping " + tmp);
            if (tmp != null && tmp.getSprite() != null)
                images[getSelectedSlot()].overrideSprite = tmp.getSprite();
            else
                images[getSelectedSlot()].overrideSprite = emptySlotSprite;
            equipped[num] = picked[getSelectedSlot() - 1];
            picked[getSelectedSlot() - 1] = tmp;
        }       
    }

    public int getSelectedSlot()
    {
        return lastSelected;
    }

    void OnCollisionEnter(Collision collision)
    {
        ItemObject obj = collision.gameObject.GetComponent<ItemObject>();
        if (obj != null)
        {
            ItemClass item = obj.getItemClass();
            if (item != null && item.canBePicked(player.gameObject)) {
            if (equipped[0] == null)
            {
                equipped[0] = item;
                obj.setItemClass(null);
                equipped[0].onPickUp(player.gameObject, this);
                equipped[0].onEquip(0);
                Destroy(collision.gameObject);
            }
            else if (equipped[1] == null)
            {
                equipped[1] = item;
                obj.setItemClass(null);
                equipped[1].onPickUp(player.gameObject, this);
                equipped[1].onEquip(0);
                Destroy(collision.gameObject);
            }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        //Inventory Visible
		if (isInventoryOpen())
        {
            ShowInventory();
            ManageInventory();
        }
        else
        //Inventory Invisible
        {
            HideInventory();
            ManageUsingItems();
        }
	}
}
