using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour {

    ItemClass item;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (item != null)
        {
            item.onUpdate(gameObject);
        }
	}

    public void setItemClass(ItemClass itc)
    {
        item = itc;
    }

    public ItemClass getItemClass()
    {
        return item;
    }
}
