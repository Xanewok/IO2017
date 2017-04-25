using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for rapier type item.
/// -Forward attacking dash
/// </summary>
public class RapierSword : AnimatorControlledObject {

    /// <summary>
    /// Number of uses before weapon destroys.
    /// Use negative for infinite durability
    /// </summary>
    [Tooltip("Durability. Negative for INF")]
    public int durability = 20;
    /// <summary>
    /// Blade script
    /// </summary>
    public RapierBlade blade;

    protected override void onStart()
    {
        base.onStart();
        meshRenderer = blade.gameObject.GetComponent<MeshRenderer>();
    }

    public override void onDropDown(GameObject player, Inventory inventory)
    {
        base.onDropDown(player, inventory);
        blade.onDrop();
    }

    public override void onPickUp(GameObject player, Inventory inventory)
    {
        base.onPickUp(player, inventory);
        blade.onPick(player);
    }

    public override void onEquip(int hand)
    {
        base.onEquip(hand);
        blade.onEquip();
    }

    public override void onUnEquip()
    {
        base.onUnEquip();
        blade.onUnEquip();
    }

    public void loseDurability()
    {
        durability--;
        if (durability == 0)
        {
            this.removeObject();
            blade.onRemove();
            Destroy(gameObject);
        }
        else if (durability < 0) durability = -1;
    }
}
