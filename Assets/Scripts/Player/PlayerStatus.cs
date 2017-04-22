using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Status component for player character
/// </summary>
public class PlayerStatus : Status
{
    /// <summary>
    /// Below what health should hope aura be active
    /// </summary>
    [Tooltip("Below what health should hope aura be active")]
    public float healthThreshold = 25f;

    public override void onStart()
    {
    }

    protected override void fillAura(int aura)
    {
        if (aura == Status.Hope)
        {
            simpleAuras[aura] = new HopeAura(this, healthThreshold);
        }
        else base.fillAura(aura);
    }
}
