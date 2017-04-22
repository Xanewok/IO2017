using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple generic class for auras that show its value under certain condition.
/// </summary>
/// <typeparam name="T">type of auras value</typeparam>
public abstract class ConditionAura<T> : AuraContainer<T>
{
    /// <summary>
    /// Value to be returned when condition is not fulfilled
    /// </summary>
    public T startingValue;

    /// <summary>
    /// Getting value of aura, considering its condition.
    /// </summary>
    /// <returns>value of aura</returns>
    public override T getValue()
    {
        return isConditionFulfilled() ? getValueInternal() : startingValue;
    }

    /// <summary>
    /// Check if condition to get real value is fulfilled
    /// </summary>
    /// <returns>true if fulfilled, false otherwise</returns>
    protected abstract bool isConditionFulfilled();

    /// <summary>
    /// Get value, with no condition considered
    /// </summary>
    /// <returns>value</returns>
    protected abstract T getValueInternal();
}
