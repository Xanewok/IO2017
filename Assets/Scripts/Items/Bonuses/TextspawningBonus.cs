using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bonus used to show some text on player's gui.
/// </summary>
public class TextspawningBonus : BonusObject {

    /// <summary>
    /// Spawner object used to spawn text
    /// </summary>
    public TextSpawner spawner;
    /// <summary>
    /// Text to be spawned.
    /// </summary>
    public String textToSpawn = "Random text";
    
    /// <summary>
    /// OnUse method - spawning text
    /// </summary>
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
