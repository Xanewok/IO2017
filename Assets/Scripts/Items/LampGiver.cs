using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampGiver : MonoBehaviour {

    public Sprite image;

    ItemObject obj;

	// Use this for initialization
	void Start () {
        obj = gameObject.GetComponent<ItemObject>();
        LampScript lamp = new LampScript();
        lamp.image = image;
        obj.setItemClass(lamp);
        Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
