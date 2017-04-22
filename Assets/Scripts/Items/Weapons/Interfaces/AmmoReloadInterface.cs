using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for every item that can be reloaded using ammo.
/// </summary>
public interface AmmoReloadInterface {

    /// <summary>
    /// Can this item take 1 of that ammunition.
    /// </summary>
    /// <param name="ammo">Ammunition object to use</param>
    /// <returns>true if can use, false otherwise</returns>
    bool canUseAmmunition(SimpleAmmunition ammo);

    /// <summary>
    /// Give weapon this ammunition.
    /// </summary>
    /// <param name="ammo">Type of ammunition given</param>
    void addAmmunition(AmmoType ammo);
}
