using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextspawningBonus : BonusObject {

    public TextSpawner spawner;
    public String textToSpawn = "Random text";

    protected override void OnUse()
    {
        spawner.spawnText(textToSpawn);
    }

    // Use this for initialization
    void Start () {
		if (spawner == null)
        {
            spawner = gameObject.GetComponent<TextSpawner>();
        }
	}
}
