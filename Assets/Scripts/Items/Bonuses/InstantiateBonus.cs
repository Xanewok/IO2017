using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple bonus that instantiates one of given items.
/// Item is chosen randomly at using bonus. (if it's null, nothing is instantiated.
/// </summary>
public class InstantiateBonus : BonusObject {

    /// <summary>
    /// List of things that can be instantiated on Use
    /// </summary>
    [Tooltip("List of thing that can be instantiated on use")]
    public GameObject[] possibleThings;

    /// <summary>
    /// On use method: Instantiates one of given objects
    /// </summary>
    protected override void OnUse()
    {
        if (possibleThings.Length > 0)
        {
            System.Random rng= new System.Random();
            int chosenIndex = rng.Next(possibleThings.Length);
            GameObject toInstantiate = possibleThings[chosenIndex];
            if (toInstantiate != null)
            {
                Instantiate(toInstantiate, transform.position, transform.rotation);
            }
        }
    }
}
