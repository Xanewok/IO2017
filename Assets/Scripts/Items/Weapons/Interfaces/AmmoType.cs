using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Superclass for types of ammo.
/// Used to pass information about used ammo, without passing instance of ammunition.
/// </summary>
public class AmmoType {
    public int type;

    public AmmoType(int type)
    {
        this.type = type;
    }
}
