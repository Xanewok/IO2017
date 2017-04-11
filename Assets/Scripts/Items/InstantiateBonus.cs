using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBonus : BonusObject {

    [Tooltip("List of thing that can be instantiated on use")]
    public GameObject[] possibleThings;

    protected override void OnUse()
    {
        if (possibleThings.Length > 0)
        {
            System.Random rng= new System.Random();
            int chosenIndex = rng.Next(possibleThings.Length);
            GameObject toInstantiate = possibleThings[chosenIndex];
            Instantiate(toInstantiate, transform.position, transform.rotation);
        }
    }
}
