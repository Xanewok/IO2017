using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for all auras.
public abstract class AuraContainer<T> {

    public delegate void AuraChangedHandler(T oldValue, T newValue);
    public event AuraChangedHandler auraUpdated;

    // Adds aura of that type to this container
    // Returns token, used to identify instance giving this aura
    public int addAura(T value)
    {
        T oldVal = getValue();
        int returned = addAuraInternal(value);
        auraUpdated(oldVal, getValue());
        return returned;
    }

    public abstract int addAuraInternal(T value);

    // Removes instance of aura of given token
    public void delAura(int token)
    {
        T oldVal = getValue();
        delAuraInternal(token);
        auraUpdated(oldVal, getValue());
    }

    public abstract void delAuraInternal(int token);

    //Returns value of that aura
    public abstract T getValue();
}
