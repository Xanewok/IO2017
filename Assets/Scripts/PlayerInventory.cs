using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory {

    public float fadeInScale = 25f;
    public float fadeOutScale = 15f;
    public Sprite emptySlotSprite;

    PlayerControlls player;
    UnityEngine.UI.Image[] images;
    Canvas canvas;
    int lastSelected = 0;
    Item[] equipped;
    Item[] picked;

	// Use this for initialization
	void Start () {
        images = new UnityEngine.UI.Image[8];
        player = this.transform.parent.gameObject.GetComponent<PlayerControlls>();
        canvas = this.GetComponent<Canvas>();
        equipped = new Item[2];
        /*for(int i=0; i<2; i++)
        {
            equipped[i] = null;
        }*/
        picked = new Item[7];
        /*for (int i = 0; i < 7; i++)
        {
            picked[i] = null;
        }*/
        canvas.enabled = false;
        {
            int i = 0;
            foreach (Transform child in transform)
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

    void hideInventory()
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

    //Zarządzanie konkretnym przedmiotem
    void ManageItem(int num)
    {
        if (getSelectedSlot() == 0)
        {
            print("Dropping Item\n");
            if (equipped[num] != null)
                equipped[num].onDropDown(player.gameObject, this);
        }
        else
        {
            print("Swapping Item\n");
            if (equipped[num] != null)
                equipped[num].onUnEquip();
            if (picked[getSelectedSlot()] != null)
                picked[getSelectedSlot()].onEquip(num);
            Item tmp = equipped[num];
            if (tmp != null && tmp.getSprite() != null)
                images[getSelectedSlot()].sprite = tmp.getSprite();
            else
                images[getSelectedSlot()].sprite = emptySlotSprite;
            equipped[num] = picked[getSelectedSlot()];
            picked[getSelectedSlot()] = tmp;
        }       
    }

    public int getSelectedSlot()
    {
        return lastSelected;
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
            hideInventory();
        }
	}
}
