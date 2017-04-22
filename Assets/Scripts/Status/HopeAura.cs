using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Aura of hope.
/// Active only when health is below certain threshold
/// </summary>
public class HopeAura : ConditionAura<float>
{
    /// <summary>
    /// Health threshold below which aura counts.
    /// </summary>
    public float healthThreshold;

    /// <summary>
    /// Dictionary to store subAuras
    /// </summary>
    protected Dictionary<int, float> values = new Dictionary<int, float>();
    /// <summary>
    /// Last index given to any aura
    /// </summary>
    int lastNum = 0;
    /// <summary>
    /// Status of person with this aura
    /// </summary>
    protected Status playerStatus;

    public HopeAura(Status status, float healthThreshold)
    {
        this.playerStatus = status;
        this.healthThreshold = healthThreshold;
    }

    //Not the best, but it may be enough.
    public override int addAuraInternal(float value)
    {
        values.Add(lastNum, value);
        return lastNum++;
    }

    public override void delAuraInternal(int token)
    {
        values.Remove(token);
    }

    protected override float getValueInternal()
    {
        Dictionary<int, float>.Enumerator enumerator = values.GetEnumerator();
        float maxVal = 0f;
        while (enumerator.MoveNext())
        {
            maxVal = Mathf.Max(maxVal, enumerator.Current.Value);
        }
        return maxVal;
    }

    protected override bool isConditionFulfilled()
    {
        return playerStatus.getHealth() <= healthThreshold;
    }
}
